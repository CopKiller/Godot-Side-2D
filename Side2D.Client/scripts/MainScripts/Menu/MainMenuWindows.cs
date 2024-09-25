using Godot;
using System;
using System.Collections.Generic;
using LiteNetLib;
using Side2D.Models;
using Side2D.Network.Packet.Client;
using Side2D.scripts;
using Side2D.scripts.Host;

public partial class MainMenuWindows : Node
{
	// Windows
	private Window _winMenu;
	private winLogin _winLogin;
	private winRegister _winRegister;
	private winCharacter _winCharacter;
	
	private Button _btnLogin;
	private Button _btnRegister;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_winMenu = GetNode<Window>("winMenu");
		_winLogin = GetNode<winLogin>("winLogin");
		_winRegister = GetNode<winRegister>("winRegister");
		_winCharacter = GetNode<winCharacter>("winCharacter");
		
		_btnLogin = GetNode<Button>("%btnLogin");
		_btnRegister = GetNode<Button>("%btnRegister");
		
		_btnLogin.Connect(BaseButton.SignalName.Pressed, Callable.From(() =>
		{
			ShowWindow(_winLogin);
		}));
		_winLogin.Connect(Window.SignalName.CloseRequested, Callable.From(() =>
		{
			HideWindow(_winLogin);
		}));
		
		_btnRegister.Connect(BaseButton.SignalName.Pressed, Callable.From(() =>
		{
			ShowWindow(_winRegister);
		}));
		_winRegister.Connect(Window.SignalName.CloseRequested, Callable.From(() =>
		{
			HideWindow(_winRegister);
		}));
	}
	
	public void StartMenu()
	{
		_winMenu.Show();
	}
	
	public void ShowCharacterWindow()
	{
		ShowWindow(_winCharacter);
	}
	
	private void ShowWindow(Window window)
	{
		_winMenu.Hide();
		window.Show();
	}
	
	private void HideWindow(Window window)
	{
		window.Hide();
		_winMenu.Show();
	}
	
	private void HideAll()
	{
		_winMenu.Hide();
		_winLogin.Hide();
		_winRegister.Hide();
		_winCharacter.Hide();
	}
	
	
	/*var clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;
			var packet = new CPlayerLogin();
			clientPlayer.SendData(packet, DeliveryMethod.ReliableOrdered);
			QueueFree();*/
}
