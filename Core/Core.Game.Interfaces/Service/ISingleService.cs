
namespace Core.Game.Interfaces.Service;

public interface ISingleService : IDisposable
{
    void Register();
    void Start();
    void Stop();
    void Restart();
    void Update(long currentTick);
}