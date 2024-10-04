
namespace Side2D.Services.Configuration;

public interface ISingleService : IDisposable
{
    void Register();
    void Start();
    void Stop();
    void Restart();
    void Update(long currentTick);
}