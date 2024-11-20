using Core.Game.Interfaces.Attribute;
using Core.Game.Models.Enum;

namespace Side2D.Server.AttributesService.Entity;

public class AttributeEntity(int index, EntityType type) : IAttributeEntity
{
    public int Index { get; } = index;
    public EntityType Type { get; } = type;

    public virtual void Update(long currentTick) { }

    public virtual void Dispose() { }
}