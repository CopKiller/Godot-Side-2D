using Core.Game.Models.Vectors;
using Side2D.Server.TempData.Temp.Interface;

namespace Side2D.Server.TempData.Temp.Player;

public class TempMove(Vector2C lastPosition) : ITempMove
{
    private long _lastTick = 0;
    public Vector2C LastPosition { get; private set; } = lastPosition;
    private const double MovementMaxSpeed = 1.2; // 1.2 px per tick
    public long LastMovementTick { get; private set; } = 0;

    public void Update(long currentTick)
    {
        _lastTick = currentTick;
    }

    public bool CanMove(Vector2C newPosition)
    {
        if (_lastTick <= LastMovementTick)
            return true;
        
        var timeElapsed = _lastTick - LastMovementTick;
        var distance = newPosition.DistanceTo(LastPosition);
        var maxDistanceAllowed = timeElapsed * MovementMaxSpeed;
        
        if (distance > maxDistanceAllowed)
        {
            // Movement too fast
            return false;
        }

        LastPosition.SetValues(newPosition);
        LastMovementTick = _lastTick;

        return true;
    }
    
    public void Dispose()
    {
        LastPosition = Vector2C.Zero;
        LastMovementTick = 0;
    }
}