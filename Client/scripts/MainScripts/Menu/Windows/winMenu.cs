using System;
using Godot;
using Side2D.scripts.Controls;

public partial class winMenu : BaseWindow
{
	private MainMenuWindows _mainMenuWindows;
	
	// Buttons
	private Button _btnLogin;
	private Button _btnRegister;
	
	public override void _Ready()
	{
		base._Ready();
		
		_mainMenuWindows = GetNode<MainMenuWindows>("..");
		
		_btnLogin = GetNode<Button>("%btnLogin");
		_btnRegister = GetNode<Button>("%btnRegister");

		ConnectSignals();
	}
	
	private void ConnectSignals()
	{
		ConnectSignalButtonPressed(_btnLogin, () => _mainMenuWindows.ShowLoginWindow());
		ConnectSignalButtonPressed(_btnRegister, () => _mainMenuWindows.ShowRegisterWindow());
	}
	
	private void ConnectSignalButtonPressed(Button button, Action action)
	{
		button.Connect(BaseButton.SignalName.Pressed, Callable.From(action));
	}
}
