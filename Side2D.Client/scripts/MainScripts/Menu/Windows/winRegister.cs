using Godot;
using LiteNetLib;
using Side2D.Models.Validation;
using Side2D.Network.Packet.Client;
using Side2D.scripts;
using Side2D.scripts.Alert;
using Side2D.scripts.Host;
using Side2D.scripts.Network;

public partial class winRegister : Window
{
	// Input
	private LineEdit _txtUsername;
	private LineEdit _txtPassword;
	private LineEdit _txtRetypePassword;
	private LineEdit _txtEmail;
	
	// Buttons
	private Button _btnEnter;
	
	// Alert
	private AlertManager _alertManager;
	private ClientPlayer _clientPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_alertManager = ApplicationHost.Instance.GetSingleton<AlertManager>();
		_clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;
		
		_txtUsername = GetNode<LineEdit>("%txtUsername");
		_txtPassword = GetNode<LineEdit>("%txtPassword");
		_txtRetypePassword = GetNode<LineEdit>("%txtRetypePassword");
		_txtEmail = GetNode<LineEdit>("%txtEmail");
		_btnEnter = GetNode<Button>("%btnEnter");
		_btnEnter.Connect(BaseButton.SignalName.Pressed, Callable.From(Register));
	}
	
	private void Register()
	{
		var result = _txtUsername.Text.IsValidName();
		if (!result)
		{
			_alertManager.AddAlert("Invalid username format");
			return;
		}
		
		result = _txtPassword.Text.IsValidPassword();
		if (!result)
		{
			_alertManager.AddAlert("Invalid password format");
			return;
		}
		
		result = _txtEmail.Text.IsValidEmail();
		if (!result)
		{
			_alertManager.AddAlert("Invalid email format");
			return;
		}
		
		if (_txtPassword.Text != _txtRetypePassword.Text)
		{
			_alertManager.AddAlert("Password and retype password do not match");
			return;
		}
		
		var packet = new CAccountRegister()
		{
			Username = _txtUsername.Text,
			Password = _txtPassword.Text,
			Email = _txtEmail.Text
		};

		_clientPlayer.SendData(packet, DeliveryMethod.ReliableOrdered);
	}
}
