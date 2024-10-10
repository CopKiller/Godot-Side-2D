namespace Core.Game.Interfaces.Service;

public interface ITransientService : IDisposable
{
    void Register();
    void Start();
    void Stop();
    void Update(long currentTick);
}