using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Physic.Player;
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
    public PhysicPlayer(int index, PlayerModel playerModel, IPhysicService physicService)
    {
        Index = index;
        _playerModel = playerModel;
        
        _move = new PhysicMove(playerModel.Position);
        _attack = new PhysicAttack(Index);
        _attack.FinishAttack = physicService.CheckPlayerAttack;
    }
    
    private IPhysicMove _move { get; }
    private IPhysicAttack _attack { get; }
    
    public bool MovePlayer(Position newPosition)
    {
        var canMove = _move.CanMove(newPosition);

        if (canMove)
        {
            _playerModel.Position.SetPosition(newPosition);
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
        return _attack.CanAttack();
    }
    
    public override void Update(long currentTick)
    {
        _move.Update(currentTick);
        _attack.Update(currentTick);
    }

    public override void Dispose()
    {
        _move.Dispose();
        _attack.Dispose();
    }
}