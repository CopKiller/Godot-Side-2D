using Core.Game.Interfaces.Service;
using Core.Game.Interfaces.TempData.Player;

namespace Core.Game.Interfaces.TempData;

public interface ITempDataService : ISingleService
{
    int DefaultUpdateInterval { get; set; }
    Dictionary<int, ITempPlayer> TempDataList { get; }
    void AddPlayerData(int index);
    void RemovePlayerData(int index);
    ITempPlayer GetPlayerData(int index);
}