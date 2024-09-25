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
}