
using Core.Game.Interfaces.Entity;

namespace Core.Game.Interfaces.Physic.Player;

public interface IPhysicAttack : IEntity
{
    bool CanAttack();
    Action<int, int>? FinishAttack { get; set; }
}