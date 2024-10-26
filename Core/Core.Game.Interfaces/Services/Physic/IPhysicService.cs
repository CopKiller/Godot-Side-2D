using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.Service;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models;
using Core.Game.Models.Player;

namespace Core.Game.Interfaces.Physic;

public interface IPhysicService : ISingleService
{
    INetworkPhysic NetworkEvents { get; }
    int DefaultUpdateInterval { get; set; }
    void AddPlayerPhysic(int index, PlayerModel playerModel);
    void RemovePlayerPhysic(int index);
    IPhysicPlayer? GetPlayerPhysic(int index);
}