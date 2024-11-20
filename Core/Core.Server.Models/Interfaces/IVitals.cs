using Core.Server.Models.Enum;

namespace Core.Server.Models.Interfaces;

public interface IVitals : IGameEntity
{
    double Health { get; set; }
    double MaxHealth { get; set; }
    double Mana { get; set; }
    double MaxMana { get; set; }
}