using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Side2D.Models.Enum;
using Side2D.Models.Interfaces;
using Side2D.Models.Player;
using Side2D.Models.Validation;
using Side2D.Models.Vectors;

namespace Side2D.Models;

public class PlayerModel : IEntity
{
    public int Id { get; set; }
    
    public int SlotNumber { get; set; }
    
    [Required]
    [StringLength(InputValidator.MaxNameCaracteres, MinimumLength = InputValidator.MinNameCaracteres)]
    public string Name { get; set; } = string.Empty;

    public int Level { get; set; } = 1;
    public Vocation Vocation { get; set; }
    public Gender Gender { get; set; }
    
    public Direction Direction { get; set; }
    
    public Position Position { get; set; }
    
    public Vitals Vitals { get; set; }
    
    public Attributes Attributes { get; set; }
    public float JumpVelocity { get; set; } = -400.0F;
    public float Speed { get; set; } = 300.0F;
    
    
    [ForeignKey("AccountModelId")] // Nome da propriedade de navegação
    public int AccountModelId { get; set; }
    
    public AccountModel AccountModel { get; set; }
    
    public ModelException? Validate()
    {
        Name = Name.Trim();
        
        return !Name.IsValidName() ? new ModelException("Invalid name.") : null;
    }
}