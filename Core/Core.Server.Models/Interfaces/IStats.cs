using Core.Server.Models.Enum;

namespace Core.Server.Models.Interfaces;

public interface IStats : IGameEntity
{
    int Strength { get; set; }
    int Defense { get; set; }
    int Agility { get; set; }
    int Intelligence { get; set; }
    int Willpower { get; set; }
}