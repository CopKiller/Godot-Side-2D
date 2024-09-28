using System;
using System.Threading;
using Godot;
using Side2D.Models.Enum;
using Side2D.Network;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;
using Side2D.scripts.Alert;
using Side2D.scripts.MainScripts.Game;
using Side2D.scripts.Network;

namespace Side2D.scripts;

public partial class ClientManager : Node, IPacketHandler
{
    private readonly NetworkManager _networkManager;
    private readonly SceneManager _sceneManager;
    
    private Thread _networkThread;
    private bool _isConnected = false;
    
    public ClientPlayer ClientPlayer { get; }
    
    public ClientManager()
    {
        Name = nameof(ClientManager);
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
        
        RegisterPacketHandlers();
    }
    
    private void Start()
    {
        _isConnected = true;
        _networkManager.Start();
        
        _networkThread = new Thread(() =>
        {
            _networkManager.DefaultUpdateInterval = 0;
            
            while (_isConnected)
            {
                _networkManager.Update();
                Thread.Sleep(15);
            }
        });
        _networkThread.Start();
    }
    
    private void Stop()
    {
        _isConnected = false;
        _networkManager.Stop();
    }

    public void ChangeClientState(ClientState state)
    {
        switch (state)
        {
            case ClientState.Menu:
                Stop();
                Start();
                _sceneManager.LoadScene<MainMenu>();
                break;
            
            case ClientState.Character:
                if (_sceneManager.CurrentScene is MainMenu mainMenu)
                    mainMenu.MainMenuWindows.ShowCharacterWindow();
                else
                {
                    ChangeClientState(ClientState.Menu);
                    GetTree().NodeAdded += OnMainMenuReady;
                    
                    void OnMainMenuReady(Node node)
                    {
                        if (node is not MainMenu menu) return;

                        menu.Loaded += () =>
                        {
                            ClientPlayer.SendData(new CPlayerSwitchCharacter());
                            menu.MainMenuWindows.ShowCharacterWindow();
                        };
                        GetTree().NodeAdded -= OnMainMenuReady;
                    }
                }
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

    public void RegisterPacketHandlers()
    {
        ClientPacketProcessor.RegisterPacket<SClientState>(ChangeState);
        return;
        
        void ChangeState(SClientState obj)
        {
            ChangeClientState(obj.ClientState);
        }
    }

    public override void _ExitTree()
    {
        ClientPacketProcessor.UnregisterPacket<SClientState>();
    }
}