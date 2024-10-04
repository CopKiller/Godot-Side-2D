using Side2D.Models.Vectors;
using Side2D.Server.TempData.Interface;
using Side2D.Server.TempData.Temp.Interface;

namespace Side2D.Server.TempData.Temp;

public class TempMove(Vector2C lastPosition) : ITempMove
{
    public Vector2C LastPosition { get; private set; } = lastPosition;
    private const int MovementMaxSpeed = 3; // 3 px per tick
    public long LastMovementTick { get; private set; } = 0;

    public void Update(long currentTick) { }

    public bool CanMove(Vector2C newPosition, long currentTick)
    {
        var timeElapsed = currentTick - LastMovementTick;
        var distance = newPosition.DistanceTo(LastPosition);
        var maxDistanceAllowed = timeElapsed * MovementMaxSpeed;
        
        if (distance > maxDistanceAllowed)
        {
            // Aqui tu enfia algo pra fuder com o cara la no client kikar o sem vergonha
            return false;
        }

        LastPosition = newPosition;
        LastMovementTick = currentTick;

        return true;
    }
    
    public void Dispose()
    {
        LastPosition = Vector2C.Zero;
        LastMovementTick = 0;
    }
}