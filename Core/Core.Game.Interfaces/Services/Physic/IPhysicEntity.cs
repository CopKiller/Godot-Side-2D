using Core.Game.Interfaces.Entity;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;

namespace Core.Game.Interfaces.Physic;

public interface IPhysicEntity : IEntity
{
    int Index { get; }
    void Move(VectorTwo newPosition, bool isServer = false);
    void ApplyKnockback(Position attackerPosition, float impactForce);
}