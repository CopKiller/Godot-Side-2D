using Side2D.Models.Vectors;
using Side2D.Server.TempData.Temp.Interface;

namespace Side2D.Server.TempData.Temp.Player;

public class TempMove(Vector2C lastPosition) : ITempMove
{
    private long _lastTick = 0;
    public Vector2C LastPosition { get; private set; } = lastPosition;
    private const double MovementMaxSpeed = 1.2; // 3 px per tick
    public long LastMovementTick { get; private set; } = 0;

    public void Update(long currentTick)
    {
        _lastTick = currentTick;
        //Console.WriteLine($"TempMove Update: {_lastTick}");
    }

    public bool CanMove(Vector2C newPosition)
    {
        //Console.WriteLine($"TempMove CanMove: {_lastTick}");
        //Console.WriteLine($"TempMove CanMove: {LastMovementTick}");
        // Verifique se LastMovementTick foi atualizado
        if (_lastTick <= LastMovementTick)
            return true;
        
        var timeElapsed = _lastTick - LastMovementTick;
        var distance = newPosition.DistanceTo(LastPosition);
        var maxDistanceAllowed = timeElapsed * (1.2);
        
        if (distance > maxDistanceAllowed)
        {
            // Aqui tu enfia algo pra fuder com o cara la no client kikar o sem vergonha
            // notificar o network service pra kikar o cara
            return false;
        }

        LastPosition = newPosition;
        LastMovementTick = _lastTick;

        return true;
    }
    
    public void Dispose()
    {
        LastPosition = Vector2C.Zero;
        LastMovementTick = 0;
    }
}