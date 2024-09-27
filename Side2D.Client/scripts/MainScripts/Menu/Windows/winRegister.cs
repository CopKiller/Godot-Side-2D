using System;
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
	// Input fields
	private LineEdit _txtUsername;
	private LineEdit _txtPassword;
	private LineEdit _txtRetypePassword;
	private LineEdit _txtEmail;
	
	// Buttons
	private Button _btnEnter;
	
	// Alert manager and player
	private AlertManager _alertManager;
	private ClientPlayer _clientPlayer;

	public override void _Ready()
	{
		_alertManager = ApplicationHost.Instance.GetSingleton<AlertManager>();
		_clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;
		
		_txtUsername = GetNode<LineEdit>("%txtUsername");
		_txtPassword = GetNode<LineEdit>("%txtPassword");
		_txtRetypePassword = GetNode<LineEdit>("%txtRetypePassword");
		_txtEmail = GetNode<LineEdit>("%txtEmail");
		_btnEnter = GetNode<Button>("%btnEnter");

		ConnectSignals();
		UpdateSubmitButtonState();
	}

	private void ConnectSignals()
	{
		_btnEnter.Connect(BaseButton.SignalName.Pressed, Callable.From(Register));
		
		ConnectTextValidation(_txtUsername, () => _txtUsername.Text.IsValidName());
		ConnectTextValidation(_txtPassword, () => _txtPassword.Text.IsValidPassword());
		ConnectTextValidation(_txtRetypePassword, () => _txtRetypePassword.Text == _txtPassword.Text);
		ConnectTextValidation(_txtEmail, () => _txtEmail.Text.IsValidEmail());
	}

	private void ConnectTextValidation(LineEdit input, Func<bool> validationFunc)
	{
		input.Connect(LineEdit.SignalName.TextChanged, Callable.From<string>((newText) =>
		{
			UpdateValidationState(input, validationFunc());
		}));
	}

	private void UpdateValidationState(Control control, bool isValid)
	{
		control.Modulate = isValid ? new Color(0, 1, 0) : new Color(1, 0, 0);
		UpdateSubmitButtonState();
	}

	private void UpdateSubmitButtonState()
	{
		var isFormValid = 
			_txtUsername.Text.IsValidName() && 
			_txtPassword.Text.IsValidPassword() && 
			_txtRetypePassword.Text == _txtPassword.Text && 
			_txtEmail.Text.IsValidEmail();
		
		_btnEnter.Disabled = !isFormValid;
	}

	private void Register()
	{
		if (!_txtUsername.Text.IsValidName())
		{
			_alertManager.AddAlert("Invalid username format");
			return;
		}
		
		if (!_txtPassword.Text.IsValidPassword())
		{
			_alertManager.AddAlert("Invalid password format");
			return;
		}
		
		if (!_txtEmail.Text.IsValidEmail())
		{
			_alertManager.AddAlert("Invalid email format");
			return;
		}
		
		if (_txtPassword.Text != _txtRetypePassword.Text)
		{
			_alertManager.AddAlert("Password and retype password do not match");
			return;
		}

		// Create and send registration packet
		var packet = new CAccountRegister()
		{
			Username = _txtUsername.Text,
			Password = _txtPassword.Text,
			Email = _txtEmail.Text
		};
		_clientPlayer.SendData(packet, DeliveryMethod.ReliableOrdered);
	}
}
