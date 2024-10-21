using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Game.Models.Player;

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
    
    public bool RegenVitalsTemp()
    {
        var result = false;
        
        if (Health <= 0) return false;

        if (Health < MaxHealth)
        {
            Health += MaxHealth * 0.01; // 1% peer tick
            result = true;
        }

        if (Mana < MaxMana)
        {
            Mana += MaxMana * 0.01; // 1% peer tick
            result = true;
        }
        
        return result;
    }
    
    public void TakeDamage(double damage)
    {
        var newHealth = Health - damage;
        Health = newHealth < 0 ? 0 : newHealth;
    }
}