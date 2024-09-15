using System.ComponentModel.DataAnnotations;
using Side2D.Models.Interfaces;

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
    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            throw new ArgumentException("Username is required.");
        }
        
        if (Username.Length is < MinNameLength or > MaxNameLength)
        {
            throw new ArgumentException($"Username must be between {MinNameLength} and {MaxNameLength} characters.");
        }
        
        if (string.IsNullOrWhiteSpace(Password))
        {
            throw new ArgumentException("Password is required.");
        }
        
        if (Password.Length is < MinPasswordLength or > MaxPasswordLength)
        {
            throw new ArgumentException($"Password must be between {MinPasswordLength} and {MaxPasswordLength} characters.");
        }
        
        if (string.IsNullOrWhiteSpace(Email))
        {
            throw new ArgumentException("Email is required.");
        }
        
        if (Email.Length is < MinEmailLength or > MaxEmailLength)
        {
            throw new ArgumentException($"Email must be between {MinEmailLength} and {MaxEmailLength} characters.");
        }
    }
}