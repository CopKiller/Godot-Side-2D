namespace Core.Service.Interfaces.Types;

public interface IService : IDisposable
{
    public IServiceConfiguration Configuration { get; }
    void Register();
    void Start();
    void Stop();
}