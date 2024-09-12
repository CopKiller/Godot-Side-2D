using System.Numerics;
using Godot;
using LiteNetLib;
using Side2D.Models.Enum;
using Side2D.Models.Vectors;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Vector2 = Godot.Vector2;

namespace Side2D.scripts;

public partial class Player : CharacterBody2D
{
	public PlayerDataModel PlayerDataModel;
	public PlayerMoveModel PlayerMoveModel;
	public CPlayerMove CPlayerMove = new();
	
	public bool IsLocal = false;
	private bool _needSync = false;
	
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	
	private bool _isMoving = false;
	private Direction _direction = Direction.Right;
	private Vector2 _velocity = Vector2.Zero;
	private AnimatedSprite2D _animatedSprite;
	
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		UpdatePlayer();
	}

	public override void _PhysicsProcess(double delta)
	{
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
				_velocity.X = direction.X * Speed;
				_isMoving = true;
			}
			else
			{
				_velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				_isMoving = false;
			}
		}
		
		void CheckPlayerJump()
		{
			if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			{
				_velocity.Y = JumpVelocity;
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
		CPlayerMove.PlayerMoveModel = PlayerMoveModel;
		ClientManager.Instance.ClientPlayer.SendData(CPlayerMove, DeliveryMethod.Sequenced);
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

	public void UpdatePlayer()
	{
		// Interpolação suave da posição
		Position = Position.Lerp(
			new Vector2(PlayerMoveModel.Position.X, PlayerMoveModel.Position.Y),
			1f // A taxa de interpolação, ajuste conforme necessário
		);

		_velocity = new Vector2(PlayerMoveModel.Velocity.X, PlayerMoveModel.Velocity.Y);
		_direction = PlayerMoveModel.Direction;
		_isMoving = PlayerMoveModel.IsMoving;
	}


}