using Godot;
using LiteNetLib;
using Side2D.Models.Validation;
using Side2D.Network.Packet.Client;
using Side2D.scripts;
using Side2D.scripts.Alert;
using Side2D.scripts.Host;
using Side2D.scripts.Network;

public partial class winLogin : Window
{
	// Input
	private LineEdit _txtUsername;
	private LineEdit _txtPassword;
	private CheckBox _chkSaveUsername;
	private CheckBox _chkSavePassword;
	
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
		_chkSaveUsername = GetNode<CheckBox>("%chkSaveUsername");
		_chkSavePassword = GetNode<CheckBox>("%chkSavePassword");
		_btnEnter = GetNode<Button>("%btnEnter");
		_btnEnter.Connect(BaseButton.SignalName.Pressed, Callable.From(Login));
	}
	
	private void Login()
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
		
		var packet = new CPlayerLogin
		{
			Username = _txtUsername.Text,
			Password = _txtPassword.Text
		};

		_clientPlayer.SendData(packet, DeliveryMethod.ReliableOrdered);
		//QueueFree();
	}
}
