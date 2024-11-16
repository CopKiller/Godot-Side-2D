namespace Core.Game.Interfaces.Service;

public interface IServicesManager : IDisposable
{
    void Register();
    
    void Start();
    
    void Stop();
    
    void Update();
}