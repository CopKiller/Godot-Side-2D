
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Service.Logic;

public class ServiceManager(IServiceConfiguration configuration, IServiceCollection collection) : IServiceManager
{
    public IServiceConfiguration Configuration { get; } = configuration;
    private IServiceProvider? ServiceProvider { get; set; }
    private List<ISingleService> Services { get; set; } = [];
    private TimerPool? TickCounter { get; set; }
    private CancellationTokenSource? _updateCancellationTokenSource;
    
    private ILogger? Logger { get; set; }

    public void Register()
    {
        
        ServiceProvider = collection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = false,
            ValidateScopes = false
        });
        
        Services.Clear();
        
        // Obter serviço ILogger apartir do ServiceProvider
        Logger = ServiceProvider.GetService<ILogger>();
        
        TickCounter = new TimerPool(Configuration, Logger);
        
        Logger?.PrintInfo("Obtendo serviços registrados como ISingleService que são singletons...");
        foreach (var serviceDescriptor in collection)
        {
            
            if (serviceDescriptor.Lifetime != ServiceLifetime.Singleton)
            {
                continue;
            }
            
            // Verifica se o tipo do serviço implementa ISingleService
            if (typeof(ISingleService).IsAssignableFrom(serviceDescriptor.ServiceType))
            {
                // Resolve o serviço
                var singletonService = ServiceProvider.GetService(serviceDescriptor.ServiceType) as ISingleService;
        
                if (singletonService != null)
                {
                    singletonService.Register();
                    Services.Add(singletonService);
                    TickCounter.AddService(singletonService);
                }
            }
        }
    }
    
    public void Start()
    {
        Logger?.PrintInfo("Starting services...");
        Services.ForEach(service =>
        {
            service.Start();
            Logger?.PrintInfo($"Service {service.GetType().Name} started.");
        });
        
        _updateCancellationTokenSource ??= new CancellationTokenSource();
        TickCounter?.Start(_updateCancellationTokenSource.Token);
    }
    
    public void Stop()
    {
        Logger?.PrintInfo("Parando serviços...");
        _updateCancellationTokenSource?.Cancel();
    }
    
    public void Restart()
    {
        Logger?.PrintInfo("Reiniciando serviços...");
        Stop();
        Start();
    }
    
    public void Update() { }
    
    public void Dispose()
    {
        Logger?.PrintInfo("Descartando serviços...");
        
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