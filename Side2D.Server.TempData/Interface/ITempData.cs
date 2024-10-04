namespace Side2D.Server.TempData.Interface;

public interface ITempData
{
    void Update(long currentTick);
    void Dispose();
}