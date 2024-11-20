
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;
using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Repositories;
using Core.Game.Interfaces.Services.Network;
using Core.Game.Interfaces.TempData;
using Core.Game.Models.Enum;
using Infrastructure.Logger;
using Infrastructure.Network;

namespace Side2D.Server.NetworkService;
public class ServerNetworkService : Infrastructure.Network.NetworkService, INetworkService
{
    private Dictionary<int, ServerClient> Players { get; set; } = new();
    private ServerPacketProcessor? ServerPacketProcessor { get; set; }
    
    public IAccountRepository AccountRepository { get; private set; }
    public IPlayerRepository PlayerRepository { get; private set; }
    public ITempDataService TempDataService { get; private set; }
    
    private IPhysicService PhysicService { get; set; }
    
    private IAttributeService AttributeService { get; set; }
    
    private ICombatService CombatService { get; set; }
    
    public ServerNetworkService(IAccountRepository accountRepository,
                                IPlayerRepository playerRepository,
                                ITempDataService tempDataService,
                                IPhysicService physicService,
                                IAttributeService attributeService,
                                ICombatService combatService)
    {
        AccountRepository = accountRepository;
        PlayerRepository = playerRepository;
        TempDataService = tempDataService;
        PhysicService = physicService;
        AttributeService = attributeService;
        CombatService = combatService;
        ServerPacketProcessor = new ServerPacketProcessor(AccountRepository, 
                                                            PlayerRepository, 
                                                            TempDataService,
                                                            PhysicService,
                                                            AttributeService,
                                                            CombatService,
                                                            Players);
    }

    public override void Register()
    {
        base.Register();

        // Server is true, client is false
        var netManager = this.NetManager;
        if (netManager != null) netManager.UseNativeSockets = true;

        if (listener == null) return;
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
        ServerPacketProcessor?.Initialize();

        Log.Print("Server: Try to start on port " + NetworkAddress.ServerPort);

        var netManager = this.NetManager;
        var result = netManager != null && netManager.Start(NetworkAddress.ServerPort);

        if (result)
        {
            Log.Print("Server: Bind on port " + NetworkAddress.ServerPort);
        }
    }

    public override void Unregister()
    {
        var eventBasedNetListener = this.listener;
        if (eventBasedNetListener != null)
        {
            eventBasedNetListener.PeerConnectedEvent -= OnPeerConnectedEvent;
            eventBasedNetListener.PeerDisconnectedEvent -= OnPeerDisconnectedEvent;
            eventBasedNetListener.NetworkErrorEvent -= OnNetworkErrorEvent;
            eventBasedNetListener.NetworkReceiveEvent -= OnNetworkReceiveEvent;
            eventBasedNetListener.NetworkReceiveUnconnectedEvent -= OnNetworkReceiveUnconnectedEvent;
            eventBasedNetListener.NetworkLatencyUpdateEvent -= OnNetworkLatencyUpdateEvent;
            eventBasedNetListener.ConnectionRequestEvent -= OnConnectionRequestEvent;
            eventBasedNetListener.DeliveryEvent -= OnDeliveryEvent;
            eventBasedNetListener.NtpResponseEvent -= OnNtpResponseEvent;
        }

        Players.Clear();
    }

    private void OnPeerConnectedEvent(NetPeer peer)
    {
        TempDataService.AddPlayerData(peer.Id);
        var tempPlayer = TempDataService.GetPlayerData(peer.Id);
        var player = new ServerClient(peer, tempPlayer, ServerPacketProcessor);
            
        Players.Add(peer.Id, player);

        Log.Print($"Player connected: {peer.Id}");
    }
    private void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Players[peer.Id].Disconnect();
        
        // Remove player from services
        TempDataService.RemovePlayerData(peer.Id);
        PhysicService.RemovePhysicEntity(1, peer.Id, EntityType.Player);
        AttributeService.RemovePlayerAttribute(peer.Id);
        CombatService.RemovePlayerCombat(peer.Id);
        
        Players.Remove(peer.Id);
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
