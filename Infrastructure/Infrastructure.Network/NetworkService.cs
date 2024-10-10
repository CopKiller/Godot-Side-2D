
using Core.Game.Interfaces.Network;
using Infrastructure.Logger;
using LiteNetLib;

namespace Infrastructure.Network;
public abstract class NetworkService : INetworkService
{
    protected NetManager? NetManager;
    protected EventBasedNetListener? listener;

    /// <inheritdoc />
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

    /// <inheritdoc />
    public virtual void Unregister()
    {
        this.Stop();
        this.listener = null;
        this.NetManager = null;
    }

    public void Restart()
    {
        this.Stop();
        this.Start();
    }

    /// <inheritdoc />
    public virtual void Update(long currentTick)
    {
        this.NetManager?.PollEvents();
    }

    /// <inheritdoc />
    public virtual void Stop()
    {
        this.NetManager?.Stop();
        Log.Print("Shutdown.");
    }

    public void Dispose()
    {
        this.Stop();
    }
}
