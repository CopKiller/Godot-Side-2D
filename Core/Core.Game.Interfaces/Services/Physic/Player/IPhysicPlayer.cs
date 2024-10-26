
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;

namespace Core.Game.Interfaces.Physic.Player;

public interface IPhysicPlayer : IPhysicEntity
{
    bool PlayerMove(Position position);
    bool PlayerAttack();
    float DistanceTo(Position position);
    Position GetPosition();
}