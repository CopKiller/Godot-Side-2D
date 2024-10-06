namespace Side2D.Services.Configuration;

public interface ITransientService : IDisposable
{
    void Register();
    void Start();
    void Stop();
    void Update(long currentTick);
}