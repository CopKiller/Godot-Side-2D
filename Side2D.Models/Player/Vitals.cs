using System.ComponentModel.DataAnnotations.Schema;

namespace Side2D.Models.Player;

public class Vitals
{
    public int Id { get; set; }
    
    public double Health { get; set; } = 100;
    public double MaxHealth { get; set; } = 100;
    
    public double Mana { get; set; } = 100;
    public double MaxMana { get; set; } = 100;
    
    [ForeignKey("PlayerModelId")]
    public int PlayerModelId { get; set; }
    
    public PlayerModel? PlayerModel { get; set; }
    
    public void Calculate(Attributes attributes)
    {
        MaxHealth = 100 + attributes.Strength * 2;
        MaxMana = 100 + attributes.Intelligence * 2;
    }
    
    public void SetValues(Vitals? vitals)
    {
        if (vitals == null) return;
        
        Health = vitals.Health;
        MaxHealth = vitals.MaxHealth;

        Mana = vitals.Mana;
        MaxMana = vitals.MaxMana;
    }
}