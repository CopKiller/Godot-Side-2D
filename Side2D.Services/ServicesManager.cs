using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Side2D.Logger;
using Side2D.Services.Configuration;

namespace Side2D.Services;

public sealed class ServicesManager : IDisposable
{
    private const int DefaultUpdateInterval = 5;

    private IServiceProvider? ServiceProvider { get; set; }             // --> Provider de serviços
    private List<ISingleService>? Services { get; set; }                // --> Coleção de serviços unicos
    
    private CancellationTokenSource? _updateCancellationTokenSource;    // --> Token de cancelamento

    public ServicesManager()
    {
        
    }

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
    }
    
    public void Restart()
    {
        Log.PrintInfo("Reiniciando serviços...");
        
        Stop();
        Start();
    }
    
    public void Update()
    {
        //Log.PrintInfo("Atualizando serviços...");
        
        if (Services == null) return;
        
        _updateCancellationTokenSource?.Cancel();
        _updateCancellationTokenSource?.Dispose();
        _updateCancellationTokenSource = new CancellationTokenSource();
        
        UpdateService(_updateCancellationTokenSource.Token);
        return;

        void UpdateService(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var service in Services)
                {
                    service.Update();
                }
                
                // Calcula o tempo gasto no processamento e ajusta o sleep adequadamente
                var elapsed = stopwatch.ElapsedMilliseconds;
                var remainingTime = DefaultUpdateInterval - elapsed; // 20ms é o intervalo desejado para a thread
                
                if (remainingTime > 0)
                {
                    Thread.Sleep((int)remainingTime);
                }
                
                stopwatch.Restart(); // Reinicia o cronômetro da thread
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
            IDisposable? disposable = service;
            disposable.Dispose();
        }
    }
}