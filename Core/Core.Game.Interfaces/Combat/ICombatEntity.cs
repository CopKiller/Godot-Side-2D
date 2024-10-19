using Core.Game.Interfaces.Entity;

namespace Core.Game.Interfaces.Combat;

public interface ICombatEntity : IEntity
{
    bool GetCombatState();
    void SetCombatState(bool inCombat);
}