using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;
using Godot;
using LiteNetLib;
using Side2D.Host;
using Infrastructure.Logger;
using Infrastructure.Network.CustomDataSerializable;
using Infrastructure.Network.Packet.Client;
using Side2D.scripts.Network;

namespace Side2D.scripts.MainScripts.Game;

public partial class Player : CharacterBody2D
{
	// Seção específica de diretórios de arquivos.
	private string _spritePath => $"res://scenes/Game/Vocation/{PlayerDataModel.Vocation.ToString()}/{PlayerDataModel.Gender.ToString().ToLower()}.tres";
	private bool Loaded { get; set; } = false;
	public bool IsLocal { get; set; } = false;
	
	private ClientPlayer _clientPlayer;
	public PlayerDataModel PlayerDataModel;
	
	private readonly CPlayerMove _cPlayerMove = new();
	private readonly CPlayerAttack _cPlayerAttack = new();
	
	private ulong _lastAttackTime = 0;
	
	private bool _isMoving = false;
	private Direction _direction = Direction.Right;
	private Vector2 _velocity = Vector2.Zero;
	
	private bool _isAttacking = false;
	
	// Children's
	private AnimatedSprite2D _animatedSprite;
	private Panel _panelBg;
	private Label _lblName;
	
	
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_panelBg = GetNode<Panel>("panelBg");
		_lblName = GetNode<Label>("lblName");

		if (IsLocal)
		{
			_clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;
			
			//var gameBars = GetTree().CurrentScene.GetNode<GameBars>("%GameBars");
			
			//gameBars.UpdateBars(PlayerDataModel.Vitals);
		}

		UpdatePlayer();
		
		Loaded = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Loaded) return;
		
		ProcessPlayerInput();
		ProcessPlayerGravity(delta);
		ProcessLocalPlayerSync();
		ProcessPlayerMovement();
		ProcessPlayerAttack();
		ProcessPlayerFrame();
	}
	
	private void ProcessPlayerInput()
	{
		if (!IsLocal) return;
		
		CheckPlayerMove();
		CheckPlayerJump();
		CheckPlayerAttack();
		return;

		void CheckPlayerMove()
		{
			_velocity = Velocity;
			var direction = Input.GetVector("move_left", "move_right", "move_jump", "move_down");
			if (direction.X != 0) 
			{
				_velocity.X = direction.X * PlayerDataModel.Speed;
				_isMoving = true;
			}
			else
			{
				_velocity.X = Mathf.MoveToward(Velocity.X, 0, PlayerDataModel.Speed);
				_isMoving = false;
			}
		}
		
		void CheckPlayerJump()
		{
			if (Input.IsActionPressed("move_jump") && IsOnFloor())
			{
				_velocity.Y = PlayerDataModel.JumpVelocity;
			}
		}

		void CheckPlayerAttack()
		{
			if (!Input.IsActionPressed("attack_action")) return;
			
			if (_isAttacking) return;
			SetPlayerAttack(true);
			_clientPlayer.SendData(_cPlayerAttack, DeliveryMethod.Sequenced);
		}
	}
	
	private void SetPlayerAttack(bool isAttacking)
	{
		_isAttacking = isAttacking;
		
		if (_isAttacking)
			_lastAttackTime = Time.GetTicksMsec() + 1000;
	}
	
	private void SetPlayerVitals()
	{
		// ...
	}
	
	private void ProcessLocalPlayerSync()
	{
		if (!IsLocal) return;

		// Checar mudanças pequenas também
		var hasPositionChanged = Position != new Vector2(PlayerDataModel.Position.X, PlayerDataModel.Position.Y);
		var hasVelocityChanged = _velocity != new Vector2(PlayerDataModel.Position.Velocity.X, PlayerDataModel.Position.Velocity.Y);

		if (!hasPositionChanged && !hasVelocityChanged && _direction == PlayerDataModel.Position.Direction &&
		    _isMoving == PlayerDataModel.Position.IsMoving) return;
		
		// Atualiza Position
		PlayerDataModel.Position.Velocity = new Microsoft.Xna.Framework.Vector2(_velocity.X, _velocity.Y);
		PlayerDataModel.Position.SetValues(Position.X, Position.Y);
		PlayerDataModel.Position.Direction = _direction;
		PlayerDataModel.Position.IsMoving = _isMoving;

		// Envia a atualização
		_cPlayerMove.Direction = _direction;
		_cPlayerMove.Position = new Microsoft.Xna.Framework.Vector2(Position.X, Position.Y);
		_cPlayerMove.Velocity = new Microsoft.Xna.Framework.Vector2(_velocity.X, _velocity.Y);
		_clientPlayer.SendData(_cPlayerMove, DeliveryMethod.Sequenced);
	}
	
	private void ProcessPlayerMovement()
	{
		Velocity = _velocity;
		
		MoveAndSlide();
	}
	
	private void ProcessPlayerAttack()
	{
		if (!_isAttacking) return;
		
		if (Time.GetTicksMsec() > _lastAttackTime)
			_isAttacking = false;
	}

	private void ProcessPlayerGravity(double delta)
	{
		// Add the gravity.
		if (!IsOnFloor())
		{
			_velocity += GetGravity() * (float)delta;
		}
	}
	
	private void ProcessPlayerFrame()
	{
		string prefix;
		
		if (_isMoving)
		{
			prefix = "move";
			_direction = _velocity.X > 0 ? Direction.Right : Direction.Left;
			
			prefix = _isAttacking ? "attack" : prefix;
		}
		else
		{
			prefix = _isAttacking ? "attack" : "idle";
		}
		
		_animatedSprite.Play($"{prefix}_{_direction.ToString().ToLower()}");
	}

	private void UpdatePlayer()
	{
		UpdatePlayerMove();
		UpdatePlayerData();
		UpdateCamera();
	}
	
	private void UpdateCamera()
	{
		if (!IsLocal)
			GetNode<Camera2D>(nameof(Camera2D)).QueueFree();
	}

	public void UpdatePlayerMove()
	{
		Position = Loaded 
			? Position.Lerp(new Vector2(PlayerDataModel.Position.X, PlayerDataModel.Position.Y), 0.1f) 
			: new Vector2(PlayerDataModel.Position.X, PlayerDataModel.Position.Y);

		_velocity = new Vector2(PlayerDataModel.Position.Velocity.X, PlayerDataModel.Position.Velocity.Y);
		_direction = PlayerDataModel.Position.Direction;
		_isMoving = PlayerDataModel.Position.IsMoving;
	}
	
	public void UpdateImpact(Vector2 impact)
	{
		PlayerDataModel.Position?.SetValues(impact.X, impact.Y);
		
		Position = impact;
	}
	
	private void UpdatePlayerData()
	{
		// Name
		UpdateName();
		// Vocation
		UpdateVocation();
    
		return;
    
		void UpdateName()
		{
			_lblName.Text = PlayerDataModel.Name;
			
			_lblName.ItemRectChanged += UpdateNameBackground;

			return;
				
			void UpdateNameBackground()
			{
				_panelBg.Size = new Vector2(_lblName.Size.X + 20, _panelBg.Size.Y);
				_panelBg.Position = new Vector2(-_panelBg.Size.X / 2, _panelBg.Position.Y);
				_lblName.ItemRectChanged -= UpdateNameBackground;
			};
		}

		void UpdateVocation()
		{
			var vocation = PlayerDataModel.Vocation.ToString();
			var gender = PlayerDataModel.Gender.ToString().ToLower();
			Log.PrintInfo($"Vocation: {vocation}");
			_animatedSprite.SpriteFrames = GD.Load<SpriteFrames>(_spritePath);
		}
	}
	
	public void Attack(bool isAttacking, AttackType attackType)
	{
		if (attackType == AttackType.Basic)
		{
			SetPlayerAttack(isAttacking);
		}
		// ...
	}
}