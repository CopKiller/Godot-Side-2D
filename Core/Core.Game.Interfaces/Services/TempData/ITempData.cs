namespace Core.Game.Interfaces.TempData;

public interface ITempData
{
    void Update(long currentTick);
    void Dispose();
}