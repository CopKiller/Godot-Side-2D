namespace Core.Database.Interfaces;

public interface IDatabaseException
{
    bool IsError { get; }
    
    string ToString();
}