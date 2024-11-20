using Core.Database.Interfaces;
using Core.Game.Interfaces.Result;

namespace Server.Service.Database.Results;

public class Result<T>(T? value, IDatabaseException? error) : IResult<T>
{
    public T? Value { get; } = value;
    public IException? Error { get; } = error;
}
