using System.Runtime.Serialization;
using Core.Game.Interfaces.Data.Vocations;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;

namespace Infrastructure.Data.Vocations.Contract;


[DataContract] public class VocationContractData : IVocationContractData
{
    [DataMember] public int Id { get; set; }
    [DataMember] public Vocation Vocation { get; set; } = Vocation.None;
    [DataMember] public Attributes Attributes { get; set; } = new();
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
                Attributes.Strength = 5;
                Attributes.Defense = 5;
                break;
            case Vocation.Mage:
                Attributes.Intelligence = 5;
                Attributes.Willpower = 5;
                break;
            case Vocation.Assassin:
                Attributes.Agility = 5;
                Attributes.Strength = 3;
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
        Vitals.Calculate(Attributes);
    }
}