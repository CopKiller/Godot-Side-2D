using Side2D.Server.TempData.Temp.Interface;
using Side2D.Services.Configuration;

namespace Side2D.Server.TempData.Interface;

public interface ITempDataService : ISingleService
{
    int DefaultUpdateInterval { get; set; }
    Dictionary<int, ITempPlayer> TempDataList { get; }
    void AddPlayerData(int index);
    void RemovePlayerData(int index);
    ITempPlayer GetPlayerData(int index);
}