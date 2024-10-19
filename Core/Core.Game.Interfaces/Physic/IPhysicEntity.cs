using Core.Game.Interfaces.Entity;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;

namespace Core.Game.Interfaces.Physic;

public interface IPhysicEntity : IEntity
{
    void Move(Vector2C newPosition);
}