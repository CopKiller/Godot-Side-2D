
using Core.Game.Interfaces.Entity;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;

namespace Core.Game.Interfaces.Physic.Player;

public interface IPhysicMove : IEntity
{
    bool CanMove(Position position);
}