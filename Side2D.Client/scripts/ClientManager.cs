using System;
using System.Threading;
using Godot;
using Side2D.Models.Enum;
using Side2D.Network;
using Side2D.scripts.Network;

namespace Side2D.scripts;

public partial class ClientManager : Node
{
    private ClientPacketProcessor _packetProcessor;
    private NetworkManager _networkManager;
    private SceneManager _sceneManager;
    private Thread _networkThread;
    
    public ClientPlayer ClientPlayer { get; private set; }
    
    public void Start()
    {
        var packetProcessor = new ClientPacketProcessor();
        var clientNetworkService = new ClientNetworkService(packetProcessor);
        ClientPlayer = new ClientPlayer(packetProcessor);
        _networkManager = new NetworkManager(clientNetworkService);
        _sceneManager = new SceneManager();
        
        clientNetworkService.CurrentPeerConnectedEvent += ClientPlayer.OnLocalPeerConnected;
        clientNetworkService.RemotePeerConnectedEvent += ClientPlayer.OnRemotePeerConnected;
        clientNetworkService.CurrentPeerDisconnectedEvent += ClientPlayer.OnLocalPeerDisconnected;
        clientNetworkService.RemotePeerDisconnectedEvent += ClientPlayer.OnRemotePeerDisconnected;
        
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
    
    public void ChangeClientStateDeferred(ClientState state)
    {
        CallDeferred(nameof(ChangeClientState), (byte)state);
    }
    
    public void ChangeClientState(ClientState state)
    {
        switch (state)
        {
            case ClientState.Menu:
                break;
            case ClientState.NewCharacter:
                break;
            case ClientState.Game:
                _sceneManager.LoadScene<Game>();
                break;
            case ClientState.None:
                GetTree().Quit();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}