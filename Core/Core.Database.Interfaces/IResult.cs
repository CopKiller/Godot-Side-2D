namespace Core.Database.Interfaces;

public interface IResult<T> where T : class
{
    T? Value { get; }
    IDatabaseException? Error { get; }
}