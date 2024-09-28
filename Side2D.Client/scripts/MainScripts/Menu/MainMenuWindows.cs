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
	private winMenu _winMenu;
	private winLogin _winLogin;
	private winRegister _winRegister;
	private winCharacter _winCharacter;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_winMenu = GetNode<winMenu>(nameof(winMenu));
		_winLogin = GetNode<winLogin>(nameof(winLogin));
		_winRegister = GetNode<winRegister>(nameof(winRegister));
		_winCharacter = GetNode<winCharacter>(nameof(winCharacter));
	}
	
	public void ShowMenuWindow()
	{
		HideAll();
		_winMenu.ShowWindow();
	}
	
	public void ShowCharacterWindow()
	{
		_winMenu.CloseWindow();
		
		_winCharacter.ShowWindow();
	}
	
	public void ShowLoginWindow()
	{
		_winMenu.CloseWindow();
		
		_winLogin.ShowWindow();
	}
	
	public void ShowRegisterWindow()
	{
		_winMenu.CloseWindow();
		
		_winRegister.ShowWindow();
	}
	
	private void HideAll()
	{
		_winMenu.Hide();
		_winLogin.Hide();
		_winRegister.Hide();
		_winCharacter.Hide();
	}
}
