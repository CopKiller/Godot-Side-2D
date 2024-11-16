using Core.Game.Interfaces.Services.Network;
using Infrastructure.Logger;
using LiteNetLib;

namespace Infrastructure.Network;
public abstract class NetworkService
{
    protected NetManager? NetManager;
    protected EventBasedNetListener? listener;
    
    public virtual void Register()
    {
        this.listener = new EventBasedNetListener();
        this.NetManager = new NetManager(this.listener)
        {
            AutoRecycle = true,
            EnableStatistics = false,
            UnconnectedMessagesEnabled = false
        };
    }

    public virtual void Start()
    {
        NetManager?.Start();
    }
    
    public virtual void Unregister()
    {
        this.listener = null;
        this.NetManager = null;
    }

    public void Restart()
    {
        this.Stop();
        this.Start();
    }
    
    public virtual void Update(long currentTick)
    {
        this.NetManager?.PollEvents();
    }
    
    public virtual void Stop()
    {
        this.NetManager?.Stop();
    }

    public void Dispose()
    {
        
    }
}
