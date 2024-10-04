using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Side2D.Logger;
using Side2D.Services.Configuration;

namespace Side2D.Services;

public sealed class ServicesManager : IDisposable
{
    private const int DefaultUpdateInterval = 5;
    private readonly Stopwatch _tickCounter = new();

    private IServiceProvider? ServiceProvider { get; set; }             // --> Provider de serviços
    private List<ISingleService>? Services { get; set; }                // --> Coleção de serviços unicos
    
    private CancellationTokenSource? _updateCancellationTokenSource;    // --> Token de cancelamento

    public void Register()
    {
        Log.PrintInfo("Registrando serviços...");
        
        var serviceCollection = new Services();
        
        var services = serviceCollection.GetServices();
        
        var servicesTypes = services.Select(x => x.ServiceType);
        
        ServiceProvider = services.BuildServiceProvider();
        Services ??= [];
        
        foreach (var serviceType in servicesTypes)
        {
            var instance = ServiceProvider.GetService(serviceType);

            if (instance is not ISingleService singletonService) continue;
             
            singletonService.Register();
            Services.Add(singletonService);
        }
    }
    
    public void Start()
    {
        Log.PrintInfo("Iniciando serviços...");
        
        _tickCounter.Start();
        
        if (Services == null) return;
        
        foreach (var service in Services)
        {
            service.Start();
        }
    }
    
    public void Stop()
    {
        Log.PrintInfo("Parando serviços...");
        
        _updateCancellationTokenSource?.Cancel();
        
        if (Services == null) return;
        
        foreach (var service in Services)
        {
            service.Stop();
        }
        
        _tickCounter.Stop();
    }
    
    public void Restart()
    {
        Log.PrintInfo("Reiniciando serviços...");
        
        Stop();
        Start();
    }
    
    public void Update()
    {
        if (Services == null) return;

        if (_updateCancellationTokenSource == null || _updateCancellationTokenSource.IsCancellationRequested)
        {
            _updateCancellationTokenSource?.Dispose();
            _updateCancellationTokenSource = new CancellationTokenSource();
        }
    
        Task.Run(() => UpdateServiceAsync(_updateCancellationTokenSource.Token));
    }
    
    private async Task UpdateServiceAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var startTick = _tickCounter.ElapsedMilliseconds;

            foreach (var service in Services)
            {
                service.Update(startTick);  // Realiza o update no serviço
            }

            var endTick = _tickCounter.ElapsedMilliseconds;
            var elapsed = endTick - startTick;
            var remainingTime = DefaultUpdateInterval - elapsed;

            if (remainingTime > 0)
            {
                // Espera o tempo necessário sem bloquear a thread
                await Task.Delay((int)remainingTime, cancellationToken);
            }
        }
    }

    
    public void Dispose()
    {
        Log.PrintInfo("Finalizando serviços...");
    
        _updateCancellationTokenSource?.Cancel();
        _updateCancellationTokenSource?.Dispose();
    
        if (Services == null) return;
    
        foreach (var service in Services)
        {
            if (service is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}