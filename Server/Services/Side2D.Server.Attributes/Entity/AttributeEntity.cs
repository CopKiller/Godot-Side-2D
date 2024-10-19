using Core.Game.Interfaces.Attribute;
using Core.Game.Models.Enum;

namespace Side2D.Server.Attributes.Entity;

public class AttributeEntity : IAttributeEntity
{
    public virtual EntityType Type { get; } = EntityType.None;
    
    public virtual void Update(long currentTick)
    {
        
    }

    public virtual void Dispose()
    {
        
    }
}