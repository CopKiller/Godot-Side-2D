using System.ComponentModel.DataAnnotations.Schema;

namespace Side2D.Models.Player;

public class Attributes
{
    public int Id { get; set; }
    public int Strength { get; set; }
    public int Defense { get; set; }
    public int Agility { get; set; }
    public int Intelligence { get; set; }
    public int Willpower { get; set; }
    
    [ForeignKey("PlayerModelId")]
    public int PlayerModelId { get; set; }
    
    public PlayerModel PlayerModel { get; set; }
    
    public Attributes()
    {
        Strength = 1;
        Defense = 1;
        Agility = 1;
        Intelligence = 1;
        Willpower = 1;
    }
    
    public void Reset()
    {
        Strength = 1;
        Defense = 1;
        Agility = 1;
        Intelligence = 1;
        Willpower = 1;
    }
    
    public void SetValues(Attributes? attributes)
    {
        Strength = attributes.Strength;
        Defense = attributes.Defense;
        Agility = attributes.Agility;
        Intelligence = attributes.Intelligence;
        Willpower = attributes.Willpower;
    }
}