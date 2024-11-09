using System.Numerics;
using Core.Game.Interfaces.Physic;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Core.Game.Interfaces.Services.Physic.World;

public interface IPhysicWorld
{
    bool AddPhysicEntity(int index, Vector2 position);
    
    bool RemovePhysicEntity(int index);
    
    bool RemovePhysicEntity(IPhysicEntity physicEntity);
    
    void Step(float delta, int velocityIterations = 8, int positionIterations = 3);
}