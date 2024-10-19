using Core.Game.Models.Player;

namespace Core.Game.Interfaces.Attribute.Player;

public interface IAttributePlayer : IAttributeEntity
{
    void TakeDamage(double damage);
    double GetDamage();
}