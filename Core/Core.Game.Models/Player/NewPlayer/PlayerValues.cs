using Core.Game.Models.Enum;

namespace Core.Game.Models.Player.NewPlayer;

internal class PlayerValues
{
    public Position Position { get; set; } = new();
    
    public Vitals Vitals { get; set; } = new();
    
    public Attributes Attributes { get; set; } = new();
    
    
    public float JumpVelocity { get; set; } = -400.0F;
    public float Speed { get; set; } = 300.0F;
    
    public PlayerValues(Vocation vocation)
    {
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
        }
        
        // Set the position
        Position.X = 300;
        Position.Y = 400;
        
        // Set the vitals based on the vocation
        Vitals.Calculate(Attributes);
    }
}