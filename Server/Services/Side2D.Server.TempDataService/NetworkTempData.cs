using Core.Game.Interfaces.Services.Network.NetworkEventServices.TempData;
using Core.Game.Models;
using Core.Game.Models.Enum;

namespace Side2D.Server.TempDataService;

public class NetworkTempData : INetworkTempData
{
    public event UpdatePlayerDelegate? OnDbSavePlayer;
    public event OnServerClientState? OnServerClientState;
    
    public void UpdatePlayer(PlayerModel player)
    {
        OnDbSavePlayer?.Invoke(player);
    }
    
    public void ServerClientState(int playerIndex, ClientState state)
    {
        OnServerClientState?.Invoke(playerIndex, state);
    }
}