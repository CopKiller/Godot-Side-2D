using Core.Game.Interfaces.Result;

namespace Side2D.Server.Repository.Results;

public class Result<T>(T? value, IException? error) : IResult<T>
{
    public T? Value { get; } = value;
    public IException? Error { get; } = error;
}
