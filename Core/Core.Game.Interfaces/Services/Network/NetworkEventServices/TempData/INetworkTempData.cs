using Core.Game.Models;
using Core.Game.Models.Enum;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.TempData;

public interface INetworkTempData
{
    event UpdatePlayerDelegate? OnDbSavePlayer;
    event OnServerClientState? OnServerClientState;
    
    void UpdatePlayer(PlayerModel player);
    void ServerClientState(int playerIndex, ClientState state);
}