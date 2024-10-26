using Core.Game.Interfaces.Combat.Player;
using Core.Game.Interfaces.Service;
using Core.Game.Models;

namespace Core.Game.Interfaces.Combat;

public interface ICombatService : ISingleService
{
    public void AddPlayerCombat(int index, PlayerModel playerModel);
    public void RemovePlayerCombat(int index);
    public void NotifyReceivePlayerAttack(int attackerIndex, int victimIndex);
}