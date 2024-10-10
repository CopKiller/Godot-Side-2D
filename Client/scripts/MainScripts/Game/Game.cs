using Godot;
using Side2D.scripts.Network;

namespace Side2D.scripts.MainScripts.Game;

public partial class Game : Node2D, IPacketHandler
{
	private GameOptions _gameOptions;
	private VirtualJoystick _virtualJoystick;
	private GameButtons _gameButtons;
	private GameBars _gameBars;
	
	public Game()
	{
		RegisterPacketHandlers();
	}
	
	public override void _Ready()
	{
		_gameOptions = GetNode<GameOptions>("%" + nameof(GameOptions));
		_virtualJoystick = GetNode<VirtualJoystick>("%" + nameof(VirtualJoystick));
		_gameButtons = GetNode<GameButtons>("%" + nameof(GameButtons));
		_gameBars = GetNode<GameBars>("%" + nameof(GameBars));
	}
	

	// Implement IPacketHandler, for handling packets from the server, and registering handlers
	public void RegisterPacketHandlers()
	{
		//ClientPacketProcessor.Instance.SubscribeReusable<SMapData>(ReceiveMapData);
	}

	public override void _ExitTree()
	{
		//ClientPacketProcessor.Instance.RemoveSubscription<SMapData>();
	}
	
	public void ShowGameOptions()
	{
		if (_gameOptions.Visible)
			_gameOptions.Hide();
		else
			_gameOptions.Show();
	}

	public override void _ShortcutInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			ShowGameOptions();
		}
	}
}