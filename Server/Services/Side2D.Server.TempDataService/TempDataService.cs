using System.Diagnostics;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.TempData;
using Core.Game.Interfaces.TempData;
using Core.Game.Interfaces.TempData.Player;
using Core.Game.Models;
using Side2D.Server.TempDataService.Temp;

namespace Side2D.Server.TempDataService;

public class TempDataService : ITempDataService
{
    public bool NeedUpdate { get; set; } = true;
    public int DefaultUpdateInterval { get; set; } = 15;
    
    public INetworkTempData NetworkEvents { get; } = new NetworkTempData();
    public Dictionary<int, ITempPlayer> TempDataList { get; private set; } = new();
    
    public void AddPlayerData(int index)
    {
        TempDataList.Add(index, new TempPlayer(index, NetworkEvents.UpdatePlayer, NetworkEvents.ServerClientState));
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

    public void Start() { }

    public void Stop() { }

    public void Restart() { }

    public void Update(long currentTick)
    {
        foreach (var tempData in TempDataList)
        {
            tempData.Value.Update(currentTick);
        }
    }
    
    public void Dispose()
    {
        foreach (var tempData in TempDataList)
        {
            tempData.Value.Dispose();
        }
    }
}