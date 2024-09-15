namespace Side2D.Server.Database;

public class DatabaseException(string msg)
{
    public string Message { get; } = msg;
}