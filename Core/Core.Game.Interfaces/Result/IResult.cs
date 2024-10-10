namespace Core.Game.Interfaces.Result;

public interface IResult<T>
{
    T? Value { get; }
    IException? Error { get; }
}