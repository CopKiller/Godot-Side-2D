using System.Runtime.Serialization;
using Core.Game.Interfaces.Data.Vocations;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;

namespace Infrastructure.Data.Vocations.Contract;


[DataContract] public class VocationContractData : IVocationContractData
{
    [DataMember] public int Id { get; set; }
    [DataMember] public Vocation Vocation { get; set; } = Vocation.None;
    [DataMember] public IStats IStats { get; set; } = new();
    [DataMember] public Vitals Vitals { get; set; } = new();
    [DataMember] public Position Position { get; set; } = new();
    [DataMember] public float JumpVelocity { get; set; } = -400.0F;
    [DataMember] public float Speed { get; set; } = 300.0F;
    
    public void Default(Vocation vocation)
    {
        Id = (byte)vocation;
        
        // Set the attributes based on the vocation
        switch (vocation)
        {
            case Vocation.Knight:
                IStats.Strength = 5;
                IStats.Defense = 5;
                break;
            case Vocation.Mage:
                IStats.Intelligence = 5;
                IStats.Willpower = 5;
                break;
            case Vocation.Assassin:
                IStats.Agility = 5;
                IStats.Strength = 3;
                break;
            case Vocation.None:
                return;
            case Vocation.Count:
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(vocation), vocation, null);
        }
        
        // Set the position
        Position.X = 300;
        Position.Y = 400;
        
        // Set the vitals based on the vocation
        Vitals.SetBy(IStats);
    }
}