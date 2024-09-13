using System.Threading;
using Side2D.Network;
using Side2D.scripts.Network;
using Side2D.scripts.Network.Packet;

namespace Side2D.scripts;

public class ClientManager
{
    public static ClientManager Instance { get; private set; }
    
    private NetworkManager _networkManager;
    private Thread _networkThread;
    
    public ClientPlayer ClientPlayer { get; private set; }
    
    private PacketReceiver _packetReceiver;
    
    public ClientManager()
    {
        Instance = this;
        var logger = new Logger();
    }
    
    public void Start()
    {
        var packetProcessor = new ClientPacketProcessor();
        var clientNetworkService = new ClientNetworkService(packetProcessor);
        ClientPlayer = new ClientPlayer(packetProcessor);
        _networkManager = new NetworkManager(clientNetworkService);
        
        clientNetworkService.CurrentPeerConnectedEvent += ClientPlayer.OnLocalPeerConnected;
        clientNetworkService.RemotePeerConnectedEvent += ClientPlayer.OnRemotePeerConnected;
        clientNetworkService.CurrentPeerDisconnectedEvent += ClientPlayer.OnLocalPeerDisconnected;
        clientNetworkService.RemotePeerDisconnectedEvent += ClientPlayer.OnRemotePeerDisconnected;
        
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