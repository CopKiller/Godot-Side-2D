using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Side2D.Models.Enum;
using Side2D.Models.Interfaces;
using Side2D.Models.Player;
using Side2D.Models.Vectors;

namespace Side2D.Models;

public class PlayerModel : IEntity
{
    private const int MaxNameLength = 20;
    private const int MinNameLength = 3;
    
    public int Id { get; set; }
    
    [Required]
    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; set; } = string.Empty;
    
    public Vocation Vocation { get; set; }
    public Gender Gender { get; set; }
    
    public Direction Direction { get; set; }
    
    public Vector2C Position { get; set; }
    
    public Vitals Vitals { get; set; }
    
    public Attributes Attributes { get; set; }
    public float JumpVelocity { get; set; } = -400.0F;
    public float Speed { get; set; } = 300.0F;
    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Name is required.");
        }
        
        if (Name.Length < MinNameLength || Name.Length > MaxNameLength)
        {
            throw new ArgumentException($"Name must be between {MinNameLength} and {MaxNameLength} characters.");
        }
    }
}