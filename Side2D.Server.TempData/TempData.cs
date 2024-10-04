using Side2D.Models.Vectors;
using Side2D.Server.TempData.Interface;
using Side2D.Server.TempData.Temp;

namespace Side2D.Server.TempData;

public class TempData : ITempData
{
    private readonly List<ITempData> _tempData = [];
    
    public TempData(Vector2C lastPosition)
    {
        _tempData.Add(new TempAttack());
        _tempData.Add(new TempMove(lastPosition));
    }
    
    public void Update(long currentTick)
    {
        foreach (var tempData in _tempData)
        {
            tempData.Update(currentTick);
        }
    }
    
    public void Dispose()
    {
        foreach (var tempData in _tempData)
        {
            tempData.Dispose();
        }
    }
}