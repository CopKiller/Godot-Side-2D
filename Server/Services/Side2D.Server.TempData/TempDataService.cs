using System.Diagnostics;
using Core.Game.Interfaces.TempData;
using Core.Game.Interfaces.TempData.Player;
using Core.Game.Models;
using Side2D.Server.TempData.Temp;

namespace Side2D.Server.TempData;

public class TempDataService : ITempDataService
{
    public int DefaultUpdateInterval { get; set; } = 1;
    
    public Dictionary<int, ITempPlayer> TempDataList { get; private set; } = new();
    private readonly Stopwatch _stopwatch = new();
    
    public void AddPlayerData(int index)
    {
        TempDataList.Add(index, new TempPlayer(index));
    }
    
    public void RemovePlayerData(int index)
    {
        TempDataList.Remove(index);
    }
    
    public ITempPlayer GetPlayerData(int index)
    {
        return TempDataList[index];
    }

    public void Register() { }

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