namespace Side2D.Server.Database.Results;

public class DatabaseException(string msg)
{
    public bool IsError { get; } = true;
    public string Message { get; } = msg;
    
    public override string ToString()
    {
        return Message;
    }
}