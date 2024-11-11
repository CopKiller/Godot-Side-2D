using Core.Game.Interfaces.Physic;
using Microsoft.Xna.Framework;

namespace Core.Game.Interfaces.Services.Physic.World;

public interface IPhysicWorld
{
    bool AddPhysicEntity(int index, Vector2 position);
    
    bool RemovePhysicEntity(int index);
    
    bool RemovePhysicEntity(IPhysicEntity physicEntity);
    
    void Step(float delta, int velocityIterations = 8, int positionIterations = 3);
}