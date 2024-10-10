namespace Core.Game.Interfaces.Result;

public interface IException
{
    bool IsError { get; }
    string Message { get; }
    
    string ToString()
    {
        return Message;
    }
}