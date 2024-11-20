namespace Core.Database.Interfaces.Account;

public interface IAccountModel
{
    string Username { get; set; }
    string Password { get; set; }
    string Email { get; set; }
    string BirthDate { get; set; }
}