using Core.Game.Models.Enum;
using Core.Game.Models.Player;

namespace Core.Game.Interfaces.Data.Vocations;

public interface IVocationContractData : IContractData
{
    Vocation Vocation { get; set; }
    Attributes Attributes { get; set; }
    Vitals Vitals { get; set; }
    Position Position { get; set; }
    float JumpVelocity { get; set; }
    float Speed { get; set; }
}