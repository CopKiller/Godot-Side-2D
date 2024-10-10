using Microsoft.Extensions.DependencyInjection;

namespace Side2D.Server.Services;

public interface IServerServices
{
    IServiceCollection GetServices();
}