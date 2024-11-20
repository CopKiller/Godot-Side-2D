namespace Core.Service.Interfaces.Types;

public interface IService : IDisposable
{
    void Register(IServiceConfiguration configuration);
    void Start();
    void Stop();
}