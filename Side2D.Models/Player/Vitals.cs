using System.ComponentModel.DataAnnotations.Schema;

namespace Side2D.Models.Player;

public class Vitals
{
    public int Id { get; set; }
    
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    
    public int Mana { get; set; }
    public int MaxMana { get; set; }
    
    [ForeignKey("PlayerModelId")]
    public int PlayerModelId { get; set; }
    
    public PlayerModel PlayerModel { get; set; }
    
    public Vitals()
    {
        Health = 100;
        MaxHealth = 100;
        
        Mana = 100;
        MaxMana = 100;
    }
    
    public void Reset()
    {
        Health = MaxHealth;
        Mana = MaxMana;
    }
    
    public void SetValues(Vitals? vitals)
    {
        Health = vitals.Health;
        MaxHealth = vitals.MaxHealth;
        
        Mana = vitals.Mana;
        MaxMana = vitals.MaxMana;
    }
}