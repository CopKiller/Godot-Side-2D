
namespace Core.Game.Interfaces.Service;

public interface ISingleService : IDisposable
{
    bool NeedUpdate { get; set; }
    int DefaultUpdateInterval { get; set; }
    void Register();
    void Start();
    void Stop();
    void Restart();
    void Update(long currentTick);
}