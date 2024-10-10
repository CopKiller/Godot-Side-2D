namespace Core.Game.Services;

public interface IServicesManager : IDisposable
{
    void Register();
    
    void Start();
    
    void Stop();
    
    void Update();
}