using Core.Game.Interfaces.Entity;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using Microsoft.Xna.Framework;

namespace Core.Game.Interfaces.Physic;

public interface IPhysicEntity : IEntity
{
    int Index { get; }
}