using Godot;
using System;
using Core.Game.Models.Enum;
using Side2D.Host;
using Side2D.scripts;

public partial class GameOptions : Window
{
	private Button _btnClose;
	private Button _btnLogout;
	private Button _btnSwitch;
	private Button _btnExit;
	
	private Action<ClientState> GoTo;
	
	public override void _Ready()
	{
		_btnClose = GetNode<Button>("%btnClose");
		_btnLogout = GetNode<Button>("%btnLogout");
		_btnSwitch = GetNode<Button>("%btnSwitch");
		_btnExit = GetNode<Button>("%btnExit");
		
		ConnectSignals();
	}
	
	private void ConnectSignals()
	{
		GoTo = (state) =>
		{
			if (state == ClientState.Game) return;
			
			ApplicationHost.Instance.GetSingleton<ClientManager>().ChangeClientState(state);
		};
		
		ConnectSignalButtonPressed(_btnClose, () => Hide());
		ConnectSignalButtonPressed(_btnLogout, () => Logout());
		ConnectSignalButtonPressed(_btnSwitch, () => SwitchCharacter());
		ConnectSignalButtonPressed(_btnExit, () => ExitGame());
		return;
		
		void ConnectSignalButtonPressed(Button button, Action action)
		{
			button.Connect(BaseButton.SignalName.Pressed, Callable.From(action));
		}
	}

	private void Logout()
	{
		GoTo(ClientState.Menu);
	}
	
	private void SwitchCharacter()
	{
		GoTo(ClientState.Character);
	}
	
	private void ExitGame()
	{
		GoTo(ClientState.None);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			Hide();
		}
	}
}
