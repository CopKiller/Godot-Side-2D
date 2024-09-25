using Godot;
using LiteNetLib;
using Side2D.Logger;
using Side2D.Models.Enum;
using Side2D.Models.Vectors;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.scripts.Host;
using Side2D.scripts.Network;

namespace Side2D.scripts.MainScripts.Game;

public partial class Player : CharacterBody2D
{
	private bool Loaded { get; set; } = false;
	public bool IsLocal { get; set; } = false;
	
	private ClientPlayer _clientPlayer;
	public PlayerDataModel PlayerDataModel;
	public PlayerMoveModel PlayerMoveModel;
	private readonly CPlayerMove _cPlayerMove = new();
	
	
	private bool _isMoving = false;
	private Direction _direction = Direction.Right;
	private Vector2 _velocity = Vector2.Zero;
	
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
			_clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;
		
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
		ProcessPlayerFrame();
	}
	
	private void ProcessPlayerInput()
	{

		if (!IsLocal) return;
		
		CheckPlayerMove();
		CheckPlayerJump();
		return;

		void CheckPlayerMove()
		{
			_velocity = Velocity;
			var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
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
			if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			{
				_velocity.Y = PlayerDataModel.JumpVelocity;
			}
		}
	}
	
	private void ProcessLocalPlayerSync()
	{
		if (!IsLocal) return;

		// Checar mudanças pequenas também
		var hasPositionChanged = Position != new Vector2(PlayerMoveModel.Position.X, PlayerMoveModel.Position.Y);
		var hasVelocityChanged = _velocity != new Vector2(PlayerMoveModel.Velocity.X, PlayerMoveModel.Velocity.Y);

		if (!hasPositionChanged && !hasVelocityChanged && _direction == PlayerMoveModel.Direction &&
		    _isMoving == PlayerMoveModel.IsMoving) return;
		
		// Atualiza PlayerMoveModel
		PlayerMoveModel.Velocity = new Vector2C(_velocity.X, _velocity.Y);
		PlayerMoveModel.Direction = _direction;
		PlayerMoveModel.IsMoving = _isMoving;
		PlayerMoveModel.Position = new Vector2C(Position.X, Position.Y);

		// Envia a atualização
		_cPlayerMove.PlayerMoveModel = PlayerMoveModel;
		_clientPlayer.SendData(_cPlayerMove, DeliveryMethod.Sequenced);
	}
	
	private void ProcessPlayerMovement()
	{
		Velocity = _velocity;
		
		MoveAndSlide();
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
		}
		else
		{
			prefix = "idle";
		}
		
		_animatedSprite.Play($"{prefix}_{_direction.ToString().ToLower()}");
		
	}

	private void UpdatePlayer()
	{
		UpdatePlayerData();
		UpdatePlayerMove();
	}

	public void UpdatePlayerMove()
	{
		Position = Loaded 
			? Position.Lerp(new Vector2(PlayerMoveModel.Position.X, PlayerMoveModel.Position.Y), 0.1f) 
			: new Vector2(PlayerMoveModel.Position.X, PlayerMoveModel.Position.Y);

		_velocity = new Vector2(PlayerMoveModel.Velocity.X, PlayerMoveModel.Velocity.Y);
		_direction = PlayerMoveModel.Direction;
		_isMoving = PlayerMoveModel.IsMoving;
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
			_panelBg.Size = new Vector2(_lblName.Size.X + 20, _panelBg.Size.Y);
			_panelBg.Position = new Vector2(-_panelBg.Size.X / 2, _panelBg.Position.Y);
		}
		void UpdateVocation()
		{
			var vocation = PlayerDataModel.Vocation.ToString();
			Log.PrintInfo($"Vocation: {vocation}");
			_animatedSprite.SpriteFrames = GD.Load<SpriteFrames>($"res://scenes/Game/Vocation/{vocation}.tres");
		}
	}
}