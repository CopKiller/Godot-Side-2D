using Core.Game.Interfaces.Service;
using Side2D.Server.TempData.Temp.Interface;

namespace Side2D.Server.TempData.Interface;

public interface ITempDataService : ISingleService
{
    int DefaultUpdateInterval { get; set; }
    Dictionary<int, ITempPlayer> TempDataList { get; }
    void AddPlayerData(int index);
    void RemovePlayerData(int index);
    ITempPlayer GetPlayerData(int index);
}