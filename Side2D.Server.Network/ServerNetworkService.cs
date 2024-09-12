

using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;
using Side2D.Logger;
using Side2D.Network;

namespace Side2D.Server.Network;
public class ServerNetworkService : NetworkService
{

    public Dictionary<int, ServerClient>? Players;

    public ServerPacketProcessor? ServerPacketProcessor { get; private set; }

    public override void Register()
    {
        base.Register();

        // Server is true, client is false
        this.NetManager.UseNativeSockets = true;

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

    public override void Start()
    {
        Players = new Dictionary<int, ServerClient>();

        ServerPacketProcessor = new ServerPacketProcessor(Players);

        ServerPacketProcessor.Initialize();

        var port = NetworkAddress.ServerPort;

        Log.Print("Server: Try to start on port " + port);

        var result = this.NetManager.Start(port);

        if (result)
        {
            Log.Print("Server: Bind on port " + port);
        }
    }

    public override void Unregister()
    {
        this.listener.PeerConnectedEvent -= OnPeerConnectedEvent;
        this.listener.PeerDisconnectedEvent -= OnPeerDisconnectedEvent;
        this.listener.NetworkErrorEvent -= OnNetworkErrorEvent;
        this.listener.NetworkReceiveEvent -= OnNetworkReceiveEvent;
        this.listener.NetworkReceiveUnconnectedEvent -= OnNetworkReceiveUnconnectedEvent;
        this.listener.NetworkLatencyUpdateEvent -= OnNetworkLatencyUpdateEvent;
        this.listener.ConnectionRequestEvent -= OnConnectionRequestEvent;
        this.listener.DeliveryEvent -= OnDeliveryEvent;
        this.listener.NtpResponseEvent -= OnNtpResponseEvent;

        Players?.Clear();

        base.Unregister();
    }

    private void OnPeerConnectedEvent(NetPeer peer)
    {
            var player = new ServerClient(peer, ServerPacketProcessor);
            Players?.Add(peer.Id, player);

        Log.Print($"Player connected: {peer.Id}");
    }
    private void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Players?[peer.Id]?.Disconnect();

        Log.Print($"{peer.Id}: {Enum.GetName(disconnectInfo.Reason)}");
    }
    private void OnNetworkErrorEvent(IPEndPoint endPoint, SocketError socketError)
    {
    }
    private void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        ServerPacketProcessor?.ReadAllPackets(reader, peer);
    }
    private void OnNetworkReceiveUnconnectedEvent(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
    }
    private void OnNetworkLatencyUpdateEvent(NetPeer peer, int latency)
    {
    }
   
    private void OnConnectionRequestEvent(ConnectionRequest request)
    {
        if (NetManager?.ConnectedPeersCount < 10)
        { /* max connections */
            request.AcceptIfKey(NetworkAddress.SecureConnectionKey);
            Log.Print("Server: Client connected: " + request.RemoteEndPoint.Address);
        }
        else
        {
            request.Reject();/* reject connection */
        }
    }
    private void OnDeliveryEvent(NetPeer peer, object userData)
    {
    }
    private void OnNtpResponseEvent(NtpPacket packet)
    {
    }
}
