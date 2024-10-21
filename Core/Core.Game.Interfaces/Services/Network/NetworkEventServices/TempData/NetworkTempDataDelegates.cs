using Core.Game.Models;
using Core.Game.Models.Enum;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.TempData;


public delegate Task<bool> UpdatePlayerDelegate(PlayerModel playerModel);

public delegate void OnServerClientState(int index, ClientState state);