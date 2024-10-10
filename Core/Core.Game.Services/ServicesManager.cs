using System.Diagnostics;
using Core.Game.Interfaces.Service;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Game.Services;

public sealed class ServicesManager : IServicesManager
{
    private const int DefaultUpdateInterval = 1;
    private readonly Stopwatch _tickCounter = new();

    private IServiceCollection? ServiceColletion { get; set; }          // --> Coleção de serviços
    private IServiceProvider? ServiceProvider { get; set; }             // --> Provider de serviços
    private List<ISingleService> Services { get; set; }                // --> Coleção de serviços unicos
    
    private CancellationTokenSource? _updateCancellationTokenSource;    // --> Token de cancelamento
    
    public ServicesManager(IServiceCollection collection)
    {
        ServiceColletion = collection;
        
        Services ??= [];
    }

    public void Register()
    {
        Log.PrintInfo("Registrando serviços...");
        
        if (ServiceColletion == null) return;
        
        var servicesTypes = ServiceColletion.Select(x => x.ServiceType);
        
        ServiceProvider = ServiceColletion.BuildServiceProvider();
        
        ServiceColletion = null;
        
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
        
        foreach (var service in Services)
        {
            service.Start();
        }
    }
    
    public void Stop()
    {
        Log.PrintInfo("Parando serviços...");
        
        _updateCancellationTokenSource?.Cancel();
        
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
        if (_updateCancellationTokenSource == null || _updateCancellationTokenSource.IsCancellationRequested)
        {
            _updateCancellationTokenSource?.Dispose();
            _updateCancellationTokenSource = new CancellationTokenSource();
        }

        // Inicia a atualização em uma nova thread
        Thread updateThread = new Thread(() => UpdateService(_updateCancellationTokenSource.Token));
        updateThread.Start();
    }

    private void UpdateService(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var startTick = _tickCounter.ElapsedMilliseconds;

            if (Services != null)
                foreach (var service in Services)
                {
                    service.Update(startTick); // Realiza o update no serviço
                }

            var endTick = _tickCounter.ElapsedMilliseconds;
            var elapsed = endTick - startTick;
            var remainingTime = DefaultUpdateInterval - elapsed;

            if (remainingTime > 0)
            {
                // Usa Thread.Sleep para aguardar o tempo necessário
                Thread.Sleep((int)remainingTime);
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