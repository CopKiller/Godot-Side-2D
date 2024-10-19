using Core.Game.Interfaces.Attribute.Player;
using Core.Game.Interfaces.Service;
using Core.Game.Models;

namespace Core.Game.Interfaces.Attribute;

public interface IAttributeService : ISingleService
{
    int DefaultUpdateInterval { get; set; }
    void AddPlayerAttribute(int index, PlayerModel playerModel);
    void RemovePlayerAttribute(int index);
    IAttributePlayer? GetPlayerAttribute(int index);
    void ReceiveCombatDamage(int attackerIndex, int victimIndex);
}