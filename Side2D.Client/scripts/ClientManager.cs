using System.Threading;
using Side2D.Network;
using Side2D.scripts.Network;
using Side2D.scripts.Network.Packet;

namespace Side2D.scripts;

public class ClientManager
{
    public static ClientManager Instance { get; private set; }
    
    private ClientNetworkService _clientNetworkService;
    private NetworkManager _networkManager;
    private ClientPacketProcessor _clientPacketProcessor;
    
    private Thread _networkThread;
    
    public ClientPlayer ClientPlayer { get; private set; } = new();
    
    private PacketReceiver _packetReceiver;
    
    public ClientManager()
    {
        Instance = this;
        new Logger();
    }
    
    public void Start()
    {
        _clientPacketProcessor = new ClientPacketProcessor();
        _clientNetworkService = new ClientNetworkService(_clientPacketProcessor);
        _networkManager = new NetworkManager(_clientNetworkService);
        
        _clientNetworkService.CurrentPeerConnectedEvent += ClientPlayer.OnLocalPeerConnected;
        _clientNetworkService.RemotePeerConnectedEvent += ClientPlayer.OnRemotePeerConnected;
        _clientNetworkService.CurrentPeerDisconnectedEvent += ClientPlayer.OnLocalPeerDisconnected;
        _clientNetworkService.RemotePeerDisconnectedEvent += ClientPlayer.OnRemotePeerDisconnected;
        
        _packetReceiver = new PacketReceiver();
        _packetReceiver.Initialize();
        
        _networkManager.Register();
        _networkManager.Start();
        
        _networkThread = new Thread(() =>
        {
            _networkManager.DefaultUpdateInterval = 0;
            
            while (true)
            {
                _networkManager.Update();
                Thread.Sleep(15);
            }
        });
        _networkThread.Start();
    }
}