using System.ComponentModel.DataAnnotations;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;

namespace Core.Database.Models;

public class AccountModel : IEntity, IAccountModel
{
    public int Id { get; set; }
    
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; }
    
    [Required]
    public required string Email { get; set; }
    
    [Required]
    public required string BirthDate { get; set; }
    
    public List<IPlayerModel> Players { get; set; } = [];
}