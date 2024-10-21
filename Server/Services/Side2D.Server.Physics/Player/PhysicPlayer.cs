using System.Diagnostics;
using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using Side2D.Server.Physics.Entity;

namespace Side2D.Server.Physics.Player;

public class PhysicPlayer : PhysicEntity,  IPhysicPlayer
{
    public override EntityType Type { get; } = EntityType.Player;
    
    public int Index { get; }
    private readonly PlayerModel _playerModel;
    
    private INetworkPhysic _networkPhysic { get; }
    
    public PhysicPlayer(int index, PlayerModel playerModel, INetworkPhysic networkPhysic, Action<int, int> CheckPlayerAttack)
    {
        Index = index;
        _playerModel = playerModel;
        _networkPhysic = networkPhysic;
        
        _move = new PhysicMove(playerModel.Position);
        _attack = new PhysicAttack(Index);
        _attack.FinishAttack = CheckPlayerAttack;
    }
    
    private IPhysicMove _move { get; }
    private IPhysicAttack _attack { get; }
    
    //Impact
    private bool _isImpacted = false;
    private float _xImpact = 0;
    private const int ImpactInterval = 50;
    private Stopwatch _impactTimer = new();
    
    public bool MovePlayer(Position newPosition)
    {
        if (_isImpacted) return false;
        
        var canMove = _move.CanMove(newPosition);

        if (canMove)
        {
            _playerModel.Position.SetPosition(newPosition);
            
            _networkPhysic.ServerUpdatePosition(Index, false);
        }
        
        return canMove;
    }
    
    public float DistanceTo(Position position)
    {
        return _playerModel.Position.DistanceTo(position);
    }
    
    public Position GetPosition()
    {
        return _playerModel.Position;
    }

    public bool Attack()
    {
        var canAttack = _attack.CanAttack();

        if (canAttack)
        {
            _networkPhysic.ServerPlayerAttack(Index, AttackType.Basic);
        }
        
        return canAttack;
    }
    
    public void ReceiveImpact(int range, Direction direction)
    {
        _isImpacted = true;
        _impactTimer.Restart();
        switch (direction)
        {
            case Direction.Left:
                _xImpact += range;
                break;
            case Direction.Right:
                _xImpact -= range;
                break;
        }
    }
    
    private void UpdateImpact()
    {
        if (!_isImpacted) return;

        if (_xImpact == 0)
        {
            _isImpacted = false;
            _impactTimer.Stop();
            return;
        }
        
        if (_impactTimer.ElapsedMilliseconds < ImpactInterval) return;
        
        var x = _playerModel.Position.X;
        var y = _playerModel.Position.Y;
        
        if (_xImpact > 0)
        {
            _xImpact -= 1;
            _playerModel.Position.SetPosition(x - 1, y);
            _networkPhysic.ServerUpdatePosition(Index, true);
        }
        else if (_xImpact < 0)
        {
            _xImpact += 1;
            _playerModel.Position.SetPosition(x + 1, y);
            _networkPhysic.ServerUpdatePosition(Index, true);
        }
    }
    
    public override void Update(long currentTick)
    {
        _move.Update(currentTick);
        _attack.Update(currentTick);
        
        UpdateImpact();
    }

    public override void Dispose()
    {
        _move.Dispose();
        _attack.Dispose();
    }
}