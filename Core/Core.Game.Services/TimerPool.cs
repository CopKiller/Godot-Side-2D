using System.Diagnostics;
using Core.Game.Interfaces.Service;
using Infrastructure.Logger;

namespace Core.Game.Services;

internal class TimerPool : IDisposable
{
    private readonly long _defaultUpdateInterval;
    private Stopwatch MainTimer { get; } = new();
    private Dictionary<ISingleService, long> ServiceLastTick { get; } = new();
    private Task? UpdateTask { get; set; }
    private CancellationTokenSource? UpdateCancellationTokenSource { get; set; }

    public TimerPool(long defaultUpdateInterval, bool start = true)
    {
        _defaultUpdateInterval = defaultUpdateInterval;
        if (start) Start();
    }

    public void Start()
    {
        MainTimer.Start();
    }
    
    public void Stop()
    {
        MainTimer.Stop();
    }

    public void AddService<T>(T service) where T : ISingleService
    {
        ServiceLastTick.TryAdd(service, 0);
    }
    
    public void StartUpdateTask(CancellationToken cancellationToken)
    {
        UpdateTask = Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var startTick = MainTimer.ElapsedMilliseconds;
                
                foreach (var service in ServiceLastTick.Keys)
                {
                    Update(service);
                }

                var elapsed = MainTimer.ElapsedMilliseconds - startTick;
                if (elapsed < _defaultUpdateInterval)
                {
                    await Task.Delay((int)(_defaultUpdateInterval - elapsed), cancellationToken);
                }
            }
        }, cancellationToken);
    }

    private void Update(ISingleService service)
    {
        if (!service.NeedUpdate) return;
        
        var tick = MainTimer.ElapsedMilliseconds;
        
        var tickCounter = tick - ServiceLastTick[service];
        if (tickCounter < service.DefaultUpdateInterval) return;

        service.Update(tick);
        ServiceLastTick[service] = tick;
    }

    public void Dispose()
    {
        Stop();
        UpdateCancellationTokenSource?.Dispose();
    }
}