namespace Side2D.Server.Database;

public class Result<T>(T? value, DatabaseException? error)
{
    public T? Value { get; } = value;
    public DatabaseException? Error { get; } = error;
}
