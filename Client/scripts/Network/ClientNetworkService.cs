using System;
using System.Net;
using System.Net.Sockets;
using Infrastructure.Logger;
using Infrastructure.Network;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Side2D.scripts.Network;
public class ClientNetworkService : NetworkService
{
    public event Action<NetPeer> RemotePeerConnectedEvent;
    public event Action<NetPeer> CurrentPeerConnectedEvent;
    public event Action<NetPeer, DisconnectInfo> RemotePeerDisconnectedEvent;
    public event Action<NetPeer> CurrentPeerDisconnectedEvent;
    public event Action<int> LatencyUpdatedEvent;
    private readonly ClientPacketProcessor _clientPacketProcessor;

    public ClientNetworkService(ClientPacketProcessor clientPacketProcessor)
    {
        _clientPacketProcessor = clientPacketProcessor;
    }

    /// <inheritdoc />
    public override void Register()
    {
        base.Register();

        // Server is true, client is false
        this.NetManager.UseNativeSockets = false;

        listener.PeerConnectedEvent += OnPeerConnectedEvent;
        listener.PeerDisconnectedEvent += OnPeerDisconnectedEvent;
        listener.NetworkErrorEvent += OnNetworkErrorEvent;
        listener.NetworkReceiveEvent += OnNetworkReceiveEvent;
        listener.NetworkReceiveUnconnectedEvent += OnNetworkReceiveUnconnectedEvent;
        listener.NetworkLatencyUpdateEvent += OnNetworkLatencyUpdateEvent;
        listener.ConnectionRequestEvent += OnConnectionRequestEvent;
        listener.DeliveryEvent += OnDeliveryEvent;
        listener.NtpResponseEvent += OnNtpResponseEvent;
    }

    /// <inheritdoc />
    public override void Start()
    {
        base.Start();
        
        var currentPeer = NetManager?.Connect(NetworkAddress.ServerAddress, NetworkAddress.ServerPort, NetworkAddress.SecureConnectionKey);

        if (currentPeer == null)
        {
            Log.PrintError("Client: Failed to connect to server");
            return;
        }
        else
            Log.PrintInfo("Client: Connected to server");

        CurrentPeerConnectedEvent?.Invoke(currentPeer);
    }

    public override void Unregister()
    {
        if (listener != null)
        {
            listener.PeerConnectedEvent -= OnPeerConnectedEvent;
            listener.PeerDisconnectedEvent -= OnPeerDisconnectedEvent;
            listener.NetworkErrorEvent -= OnNetworkErrorEvent;
            listener.NetworkReceiveEvent -= OnNetworkReceiveEvent;
            listener.NetworkReceiveUnconnectedEvent -= OnNetworkReceiveUnconnectedEvent;
            listener.NetworkLatencyUpdateEvent -= OnNetworkLatencyUpdateEvent;
            listener.ConnectionRequestEvent -= OnConnectionRequestEvent;
            listener.DeliveryEvent -= OnDeliveryEvent;
            listener.NtpResponseEvent -= OnNtpResponseEvent;
        }

        base.Unregister();
    }

    private void OnPeerConnectedEvent(NetPeer peer)
    {
        RemotePeerConnectedEvent?.Invoke(peer);
    }

    private void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        RemotePeerDisconnectedEvent?.Invoke(peer, disconnectInfo);
    }

    private void OnNetworkErrorEvent(IPEndPoint endPoint, SocketError socketError)
    {

    }

    private void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        _clientPacketProcessor?.ReadAllPackets(reader, peer);
    }

    private void OnNetworkReceiveUnconnectedEvent(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {

    }

    private void OnNetworkLatencyUpdateEvent(NetPeer peer, int latency)
    {
        LatencyUpdatedEvent?.Invoke(latency);
    }

    private void OnConnectionRequestEvent(ConnectionRequest request)
    {
        //request.AcceptIfKey(NetworkAddress.SecureConnectionKey);
        //request.Reject();
    }

    private void OnDeliveryEvent(NetPeer peer, object userData)
    {

    }

    private void OnNtpResponseEvent(NtpPacket packet)
    {

    }
}
