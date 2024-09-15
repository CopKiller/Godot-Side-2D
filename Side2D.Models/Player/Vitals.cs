using System.ComponentModel.DataAnnotations.Schema;

namespace Side2D.Models.Player;

public class Vitals
{
    public int Id { get; set; }
    
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    
    public int Mana { get; set; }
    public int MaxMana { get; set; }
}