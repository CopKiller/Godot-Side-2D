using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Game.Models.Player;

public class Attributes
{
    public int Id { get; set; }
    public int Strength { get; set; } = 1;
    public int Defense { get; set; } = 1;
    public int Agility { get; set; } = 1;
    public int Intelligence { get; set; } = 1;
    public int Willpower { get; set; } = 1;
    
    [ForeignKey("PlayerModelId")]
    public int PlayerModelId { get; set; }
    public PlayerModel PlayerModel { get; set; }
    
    public void SetValues(Attributes attributes)
    {
        Strength = attributes.Strength;
        Defense = attributes.Defense;
        Agility = attributes.Agility;
        Intelligence = attributes.Intelligence;
        Willpower = attributes.Willpower;
    }
    
    public double GetDamage()
    {
        return Strength * 1.5;
    }
}