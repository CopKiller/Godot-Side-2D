namespace Side2D.Models;

public class AccountModel
{
    public int Id { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty; // This should be hashed
    
    public string Email { get; set; } = string.Empty;
    
    public List<PlayerModel> Players { get; set; } = [];
}