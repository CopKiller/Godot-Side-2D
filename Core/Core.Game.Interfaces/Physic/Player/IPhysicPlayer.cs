
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;

namespace Core.Game.Interfaces.Physic.Player;

public interface IPhysicPlayer : IPhysicEntity
{
    int Index { get; }
    bool MovePlayer(Position position);
    bool Attack();
    float DistanceTo(Position position);
    Position GetPosition();
}