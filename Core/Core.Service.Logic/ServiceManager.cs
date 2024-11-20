
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Service.Logic;

public class ServiceManager(IServiceCollection collection) : IServiceManager
{
    private const int DefaultUpdateInterval = 1;
    private IServiceProvider? ServiceProvider { get; set; }
    private List<ISingleService> Services { get; set; } = new();
    private TimerPool? TickCounter { get; set; }
    private CancellationTokenSource? _updateCancellationTokenSource;

    public void Register(IServiceConfiguration configuration)
    {
        Log.PrintInfo("Registrando serviços...");
        
        TickCounter = new TimerPool(DefaultUpdateInterval, false);
        
        ServiceProvider = collection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = false,
            ValidateScopes = false
        });
        
        Services.Clear();
        
        Log.PrintInfo("Obtendo serviços registrados como ISingleService que são singletons...");
        foreach (var serviceType in collection.Select(x => x.ServiceType))
        {
            if (ServiceProvider.GetRequiredService(serviceType) is ISingleService singletonService)
            {
                singletonService.Register();
                Services.Add(singletonService);
                TickCounter.AddService(singletonService);
            }
        }
    }
    
    public void Start()
    {
        Log.PrintInfo("Iniciando serviços...");
        Services.ForEach(service =>
        {
            service.Start();
            Log.PrintInfo($"Serviço {service.GetType().Name} iniciado.");
        });
        TickCounter?.Start();
    }
    
    public void Stop()
    {
        Log.PrintInfo("Parando serviços...");
        _updateCancellationTokenSource?.Cancel();
        TickCounter?.Stop();
        Services.ForEach(service =>
        {
            service.Stop();
            Log.PrintInfo($"Serviço {service.GetType().Name} parado.");
        });
    }
    
    public void Restart()
    {
        Log.PrintInfo("Reiniciando serviços...");
        Stop();
        Start();
    }
    
    public void Update()
    {
        _updateCancellationTokenSource?.Cancel();
        _updateCancellationTokenSource = new CancellationTokenSource();
        TickCounter?.StartUpdateTask(_updateCancellationTokenSource.Token);
    }
    
    public void Dispose()
    {
        Log.PrintInfo("Descartando serviços...");
        
        TickCounter?.Dispose();
        
        // Dispose seguro para o ServiceProvider, caso seja IDisposable
        if (ServiceProvider is IDisposable disposableProvider)
        {
            disposableProvider.Dispose();
        }
    
        _updateCancellationTokenSource?.Dispose();
    
        foreach (var service in Services.OfType<IDisposable>())
        {
            service.Dispose();
        }
    }

}