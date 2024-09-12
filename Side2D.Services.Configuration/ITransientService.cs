namespace Side2D.Services.Configuration;

public interface ITransientService
{
    void Register();
    void Start();
    void Stop();
    void Update();
    void Dispose();
}