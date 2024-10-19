using Core.Game.Models.Enum;

namespace Core.Game.Interfaces.Entity;

public interface IEntity
{
    EntityType Type { get; }
    void Update(long currentTick);
    void Dispose();
}