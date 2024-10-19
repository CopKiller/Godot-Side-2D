using Core.Game.Interfaces.Physic;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;

namespace Side2D.Server.Physics.Entity;

public class PhysicEntity : IPhysicEntity
{
    public virtual EntityType Type { get; } = EntityType.None;
    
    public virtual void Update(long currentTick)
    {
        
    }

    public virtual void Dispose()
    {
        
    }

    public virtual void Move(Vector2C newPosition)
    {
        
        
    }
}