using System.ComponentModel.DataAnnotations;
using Core.Game.Models.Interfaces;
using Core.Game.Models.Validation;

namespace Core.Game.Models;

public class AccountModel : IEntity
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(InputValidator.MaxNameCaracteres, MinimumLength = InputValidator.MinNameCaracteres)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(InputValidator.MaxPasswordCaracteres, MinimumLength = InputValidator.MinPasswordCaracteres)]
    public string Password { get; set; } = string.Empty; // This should be hashed
    
    [Required]
    [StringLength(InputValidator.MaxEmailCaracteres, MinimumLength = InputValidator.MinEmailCaracteres)]
    public string Email { get; set; } = string.Empty;
    
    public List<PlayerModel> Players { get; set; } = [];
    
    public ModelException? Validate()
    {
        Username = Username.Trim();
        Password = Password.Trim();
        Email = Email.Trim();
        
        if (!Username.IsValidName())
        {
            return new ModelException("Username is invalid.");
        }
        
        if (!Password.IsValidPassword())
        {
            return new ModelException("Password is invalid.");
        }
        
        return !Email.IsValidEmail() ? new ModelException("Email is invalid.") : null;
    }
}