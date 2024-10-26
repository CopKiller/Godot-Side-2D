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

public partial class PhysicPlayer(int index, PlayerModel playerModel, INetworkPhysic networkPhysic, PhysicService physicService)
                     :PhysicEntity (index, EntityType.Player, playerModel.Position, networkPhysic)
                         , IPhysicPlayer
{
    private long _currentTick = 0;
    private readonly INetworkPhysic _networkPhysic = networkPhysic;

    public bool PlayerMove(Position newPosition)
    {
        var canMove = CanMove(newPosition, _currentTick);

        if (canMove)
        {
            base.Move(newPosition, false);
        }
        
        return canMove;
    }
    
    public bool PlayerAttack()
    {
        var canAttack = CanAttack();

        if (canAttack)
        {
            _networkPhysic.ServerUpdateAttack(Index, Type, false, AttackType.Basic);
        }
        
        return canAttack;
    }
    
    public float DistanceTo(Position position)
    {
        return playerModel.Position.DistanceTo(position);
    }
    
    public Position GetPosition()
    {
        return playerModel.Position;
    }
    
    public override void Update(long currentTick)
    {
        _currentTick = currentTick;
        
        UpdateAttack();
        
        base.Update(currentTick);
        
    }

    public override void Dispose() { }
}