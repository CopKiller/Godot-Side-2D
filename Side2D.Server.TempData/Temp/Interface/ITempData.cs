namespace Side2D.Server.TempData.Temp.Interface;

public interface ITempData
{
    void Update(long currentTick);
    void Dispose();
}