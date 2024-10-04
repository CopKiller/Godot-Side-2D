using System.Diagnostics;
using Side2D.Server.TempData.Interface;

namespace Side2D.Server.TempData;

public class TempDataService : ITempDataService
{
    public Dictionary<int, ITempData> TempDataList { get; private set; } = new();
    
    public int DefaultUpdateInterval = 15;
    private readonly Stopwatch _stopwatch = new();

    public void Register()
    {
        
    }

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Stop()
    {
        _stopwatch.Stop();
    }

    public void Restart()
    {
        _stopwatch.Restart();
    }

    public void Update(long currentTick)
    {
        if (DefaultUpdateInterval > 0) 
            if (_stopwatch.ElapsedMilliseconds < DefaultUpdateInterval) return;
        
        foreach (var tempData in TempDataList)
        {
            tempData.Value.Update(currentTick);
        }
        
        if (DefaultUpdateInterval > 0) 
            _stopwatch.Restart();
    }
    
    public void Dispose()
    {
        foreach (var tempData in TempDataList)
        {
            tempData.Value.Dispose();
        }
    }
}