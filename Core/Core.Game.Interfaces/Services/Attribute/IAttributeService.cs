using Core.Game.Interfaces.Attribute.Player;
using Core.Game.Interfaces.Service;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;
using Core.Game.Models;

namespace Core.Game.Interfaces.Attribute;

public interface IAttributeService : ISingleService
{
    INetworkAttribute NetworkEvents { get; }
    void AddPlayerAttribute(int index, PlayerModel playerModel);
    void RemovePlayerAttribute(int index);
    IAttributePlayer? GetPlayerAttribute(int index);
    void ReceiveCombatDamage(int attackerIndex, int victimIndex);
    void SetPlayerCombatState(int index, bool inCombat);
}