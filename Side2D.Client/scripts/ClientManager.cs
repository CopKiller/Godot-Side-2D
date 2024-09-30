using System;
using System.Threading;
using Godot;
using Side2D.Cryptography;
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
    private readonly CryptoManager _cryptoManager;
    
    private Thread _networkThread;
    private CancellationTokenSource _cancellationTokenSource;
    
    public ClientPlayer ClientPlayer { get; }
    
    public ClientState ClientState { get; private set; } = ClientState.None;
    
    public ClientManager()
    {
        _cryptoManager = new CryptoManager();
        
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
        _networkManager.Start();
        
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _networkManager.Stop());
        
        _networkThread = new Thread(() =>
        {
            _networkManager.DefaultUpdateInterval = 0;
            
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _networkManager.Update();
                Thread.Sleep(15);
            }
        });
        _networkThread.Start();
    }
    
    private void Stop()
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();  // Sinaliza que o loop deve parar
            _networkThread.Join();  // Aguarda a thread terminar
            GD.Print("Network stopped successfully.");
            
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
    
    public void RestartNetwork()
    {
        Stop();
        
        Start();
    }

    public void ChangeClientState(ClientState state)
    {
        ClientState = state;
        
        switch (state)
        {
            case ClientState.Menu:
                _sceneManager.LoadScene<MainMenu>();
                RestartNetwork();
                break;
            
            case ClientState.Character:
                if (_sceneManager.CurrentScene is MainMenu mainMenu)
                    mainMenu.MainMenuWindows.ShowCharacterWindow();
                else
                {
                    _sceneManager.LoadScene<MainMenu>();
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