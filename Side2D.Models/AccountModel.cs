using System.ComponentModel.DataAnnotations;
using Side2D.Models.Interfaces;
using Side2D.Models.Result;

namespace Side2D.Models;

public class AccountModel : IEntity
{
    private const int MaxNameLength = 20;
    private const int MinNameLength = 3;
    private const int MaxPasswordLength = 20;
    private const int MinPasswordLength = 6;
    private const int MaxEmailLength = 50;
    private const int MinEmailLength = 5;
    
    public int Id { get; set; }
    
    [Required]
    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength)]
    public string Password { get; set; } = string.Empty; // This should be hashed
    
    [Required]
    [StringLength(MaxEmailLength, MinimumLength = MinEmailLength)]
    public string Email { get; set; } = string.Empty;
    
    public List<PlayerModel> Players { get; set; } = [];
    
    public ModelException? Validate()
    {
        Username = Username.Trim();
        Password = Password.Trim();
        Email = Email.Trim();
        
        if (string.IsNullOrWhiteSpace(Username))
        {
            return new ModelException("Username is required.");
        }
        
        if (Username.Length is < MinNameLength or > MaxNameLength)
        {
            return new ModelException($"Username must be between {MinNameLength} and {MaxNameLength} characters.");
        }
        
        if (string.IsNullOrWhiteSpace(Password))
        {
            return new ModelException("Password is required.");
        }
        
        if (Password.Length is < MinPasswordLength or > MaxPasswordLength)
        {
            return new ModelException($"Password must be between {MinPasswordLength} and {MaxPasswordLength} characters.");
        }
        
        if (string.IsNullOrWhiteSpace(Email))
        {
            return new ModelException("Email is required.");
        }
        
        if (Email.Length is < MinEmailLength or > MaxEmailLength)
        {
            return new ModelException($"Email must be between {MinEmailLength} and {MaxEmailLength} characters.");
        }

        return null;
    }
}