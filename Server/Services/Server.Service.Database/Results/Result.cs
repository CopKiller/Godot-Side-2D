using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;

namespace Server.Service.Database.Results;

public class Result<T>(T? value, IDatabaseException? error) : IResult<T> where T : class
{
    public T? Value { get; } = value;
    public IDatabaseException? Error { get; } = error;
}
