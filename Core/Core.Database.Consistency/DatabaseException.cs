using Core.Database.Interfaces;

namespace Core.Database.Consistency;

public class DatabaseException(string msg) : IDatabaseException
{
    public bool IsError { get; } = true;
    private string Message { get; } = msg;
    
    public override string ToString()
    {
        return Message;
    }
}