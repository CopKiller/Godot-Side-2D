using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Game.Models.Enum;
using Core.Game.Models.Interfaces;
using Core.Game.Models.Player;
using Core.Game.Models.Validation;

namespace Core.Game.Models;

public class PlayerModel : IEntity
{
    public PlayerModel() { }
    public PlayerModel(int slotNumber, string name, Vocation vocation, Gender gender)
    {
        SlotNumber = slotNumber;
        Name = name;
        Vocation = vocation;
        Gender = gender;
        
        // Set default values based on the vocation
        /* var playerValues = new PlayerValues(vocation);
        Position = playerValues.Position;
        Vitals = playerValues.Vitals;
        Attributes = playerValues.Attributes;
        JumpVelocity = playerValues.JumpVelocity;
        Speed = playerValues.Speed; */
    }
    
    [NotMapped] public int Index { get; set; }
    public int Id { get; set; }
    public int SlotNumber { get; set; }
    
    [StringLength(InputValidator.MaxNameCaracteres, MinimumLength = InputValidator.MinNameCaracteres)]
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
    public Vocation Vocation { get; set; }
    public Gender Gender { get; set; }
    public Position Position { get; set; }
    public Vitals Vitals { get; set; }
    public Attributes Attributes { get; set; }
    public float JumpVelocity { get; set; }
    public float Speed { get; set; }
    
    
    [ForeignKey("AccountModelId")] // Nome da propriedade de navegação
    public int AccountModelId { get; set; }
    
    public AccountModel AccountModel { get; set; }
    
    public ModelException? Validate()
    {
        Name = Name.Trim();
        
        return !Name.IsValidName() ? new ModelException("Invalid name.") : null;
    }
}