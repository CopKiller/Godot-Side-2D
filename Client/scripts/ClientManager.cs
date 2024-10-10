using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Core.Game.Models.Enum;
using Godot;
using Side2D.Cryptography;
using Infrastructure.Network;
using Infrastructure.Network.Packet.Client;
using Infrastructure.Network.Packet.Server;
using Side2D.scripts.MainScripts.Game;
using Side2D.scripts.Network;

namespace Side2D.scripts;

public partial class ClientManager : Node, IPacketHandler
{
    private readonly NetworkManager _networkManager;
    private readonly SceneManager _sceneManager;
    private readonly CryptoManager _cryptoManager;
    
    private Thread _networkThread;
    private CancellationTokenSource? _cancellationTokenSource;
    
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
        
        _networkThread = new Thread(ThreadStart);
        _networkThread.Start();
    }

    private async void ThreadStart()
    {
        _networkManager.DefaultUpdateInterval = 0;
        
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        try
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _networkManager.Update(stopwatch.ElapsedMilliseconds);
                await Task.Delay(15, _cancellationTokenSource.Token);
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr("Error in network thread: ", ex);
            stopwatch.Stop();
        }
    }

    private void Stop()
    {
        if (_cancellationTokenSource is not { IsCancellationRequested: false }) return;
        
        //if (!_networkThread.IsAlive) return;
        
        _cancellationTokenSource.Cancel();  // Sinaliza que o loop deve parar
        //_networkThread.Join();  // Aguarda a thread terminar
        GD.Print("Network stopped successfully.");
            
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
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
                    GetTree().NodeAdded += OnMainMenuReady;
                    _sceneManager.LoadScene<MainMenu>();
                    
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
    
    public override void _Notification(int what)
    {
        if (what == NotificationPredelete || what == NotificationCrash || what == NotificationExitTree)
        {
            Stop();
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
        Stop();
        ClientPacketProcessor.UnregisterPacket<SClientState>();
        base._ExitTree();
    }
}