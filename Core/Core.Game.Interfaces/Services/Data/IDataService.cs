using Core.Game.Interfaces.Service;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.TempData;
using Core.Game.Interfaces.TempData.Player;

namespace Core.Game.Interfaces.Services.Data;

public interface IDataService : ISingleService
{
    Dictionary<int, ITempPlayer> TempDataList { get; }
    void AddPlayerData(int index);
    void RemovePlayerData(int index);
    ITempPlayer GetPlayerData(int index);
}