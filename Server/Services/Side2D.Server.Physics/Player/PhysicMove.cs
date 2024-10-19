using Core.Game.Interfaces.Physic.Player;
using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using Side2D.Server.Physics.Entity;

namespace Side2D.Server.Physics.Player;

public class PhysicMove(Position position) : PhysicEntity, IPhysicMove
{
    private long _lastTick = 0;
    private const double MovementMaxSpeed = 1.2; // 1.2 px per tick
    public long LastMovementTick { get; private set; } = 0;

    public override void Update(long currentTick)
    {
        _lastTick = currentTick;
    }

    public bool CanMove(Position newPosition)
    {
        if (_lastTick <= LastMovementTick)
            return true;
        
        var timeElapsed = _lastTick - LastMovementTick;
        var distance = newPosition.DistanceTo(position);
        var maxDistanceAllowed = timeElapsed * MovementMaxSpeed;
        
        if (distance > maxDistanceAllowed)
        {
            // Movement too fast
            return false;
        }
        
        LastMovementTick = _lastTick;

        return true;
    }
    
    public override void Dispose()
    {
        LastMovementTick = 0;
    }
}