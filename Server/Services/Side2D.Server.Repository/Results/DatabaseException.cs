using Core.Game.Interfaces.Result;

namespace Side2D.Server.Repository.Results;

public class DatabaseException(string msg) : IException
{
    public bool IsError { get; } = true;
    public string Message { get; } = msg;
    
    public override string ToString()
    {
        return Message;
    }
}