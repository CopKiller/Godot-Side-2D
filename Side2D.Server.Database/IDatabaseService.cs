using Microsoft.Extensions.DependencyInjection;
using Side2D.Models;
using Side2D.Server.Database.Interfaces;
using Side2D.Services.Configuration;

namespace Side2D.Server.Database;

public interface IDatabaseService : ISingleService
{
    IAccountRepository AccountRepository { get; }
    IPlayerRepository PlayerRepository { get; }
}