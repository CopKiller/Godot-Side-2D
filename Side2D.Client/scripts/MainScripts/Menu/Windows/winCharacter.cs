using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using Side2D.Host;
using Side2D.Models.Enum;
using Side2D.Models.Validation;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;
using Side2D.scripts;
using Side2D.scripts.Alert;
using Side2D.scripts.Controls;
using Side2D.scripts.MainScripts.Menu.Windows;
using Side2D.scripts.Network;

public partial class winCharacter : BaseWindow, IPacketHandler
{
	private const int MAX_FRAMES = 2;
	
	// Main Container
	private HBoxContainer _mainContainer;
	
	// Info Labels
	private Label _lblName;
	private Label _lblLevel;
	private Label _lblVocation;
	
	// Buttons
	private Button _btnCreateChar;
	private Button _btnEnterGame;
	
	// Slots
	private List<CharSlotButton> _btnSlots = []; 
	
	// Create Character
	private PanelContainer _createContainer;
	// --> Inputs
	private LineEdit _txtName;
	private OptionButton _optGender;
	private OptionButton _optVocation;
	private Button _btnCreate;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
		_mainContainer = GetNode<HBoxContainer>("%MainContainer");

		GetInfoNodes();
		GetButtonNodes();
		GetCreateNodes();

		ConnectSignals();

		RegisterPacketHandlers();
		
		PopulateOptions();
		
		UpdateSubmitButtonState();
		return;

		void GetInfoNodes()
		{
			_lblName = GetNode<Label>("%lblName");
			_lblLevel = GetNode<Label>("%lblLevel");
			_lblVocation = GetNode<Label>("%lblVocation");
		}

		void GetButtonNodes()
		{
			_btnCreateChar = GetNode<Button>("%btnCreateChar");
			_btnEnterGame = GetNode<Button>("%btnEnterGame");
		}

		void GetCreateNodes()
		{
			_createContainer = GetNode<PanelContainer>("%CreateContainer");
			_txtName = GetNode<LineEdit>("%txtName");
			_optGender = GetNode<OptionButton>("%optGender");
			_optVocation = GetNode<OptionButton>("%optVocation");
			_btnCreate = GetNode<Button>("%btnCreate");
		}
	}
	
	private void PopulateOptions()
	{
		foreach (var gender in Enum.GetValues<Gender>())
		{
			if (gender == Gender.Count) continue;
			_optGender.AddItem(gender.ToString(), (int)gender);
		}

		foreach (var vocation in Enum.GetValues<Vocation>())
		{
			if (vocation == Vocation.Count) continue;
			_optVocation.AddItem(vocation.ToString(), (int)vocation);
		}

	}

	private void ConnectSignals()
	{
		_btnCreate.Connect(BaseButton.SignalName.Pressed, Callable.From(OnCreatePressed));
		_btnCreateChar.Connect(BaseButton.SignalName.Pressed, Callable.From(OnCreateCharPressed));
		_btnEnterGame.Connect(BaseButton.SignalName.Pressed, Callable.From(OnEnterGamePressed));
		
		_txtName.Connect(LineEdit.SignalName.TextChanged, Callable.From<string>((newText) =>
		{
			UpdateValidationState(_txtName, newText.IsValidName());
		}));
		ConnectOptionValidation(_optGender, () => _optGender.Selected.IsValidGender());
		ConnectOptionValidation(_optVocation, () => _optVocation.Selected.IsValidVocation());
	}
	
	private void ConnectOptionValidation(OptionButton input, Func<bool> validationFunc)
	{
		input.Connect(OptionButton.SignalName.ItemSelected, Callable.From<int>((index) =>
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
			_optGender.Selected.IsValidGender() && 
			_optVocation.Selected.IsValidVocation();
		
		_btnCreate.Disabled = !isFormValid;
	}
	
	private void OnEnterGamePressed()
	{
		// Enter game
		var clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;
		var slot = _btnSlots.First(a => a.ButtonPressed);

		var packet = new CPlayerUseCharacter
		{
			SlotNumber = slot.SlotNumber
		};

		clientPlayer.SendData(packet);
	}
	
	private void OnCreateCharPressed()
	{
		// Open create character panel
		_createContainer.Show();
		foreach(var slot in _btnSlots)
		{
			slot.Hide();
		}
			
		_btnCreateChar.Disabled = true;
	}
	
	private void OnCreatePressed()
	{
		var alertManager = ApplicationHost.Instance.GetSingleton<AlertManager>();
		// Validate inputs
		if (!_txtName.Text.IsValidName())
		{
			alertManager.AddAlert($"Invalid name Name must have between {InputValidator.MinNameCaracteres} and {InputValidator.MaxNameCaracteres} characters");
			return;
		}

		if (!_optGender.Selected.IsValidGender())
		{
			alertManager.AddAlert($"Invalid Gender");
			return;
		}
			
		if (!_optVocation.Selected.IsValidVocation())
		{
			alertManager.AddAlert($"Invalid Vocation");
			return;
		}

		// Send Create Character Packet
		var clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;

		var packet = new CCreateCharacter()
		{
			SlotNumber = _btnSlots.First(a => a.ButtonPressed).SlotNumber,
			Name = _txtName.Text,
			Gender = (Gender)_optGender.Selected,
			Vocation = (Vocation)_optVocation.Selected
		};
			
		clientPlayer.SendData(packet);
			
		_createContainer.Hide();
	}


	public void AddCharacters(List<PlayerDataModel> playerDataModel)
	{
		GD.Print($"Adding characters to slots quantity: {playerDataModel.Count}");
		
		if (_btnSlots.Count > 0)
		{
			foreach (var btnSlot in _btnSlots)
			{
				btnSlot.QueueFree();
			}
			_btnSlots.Clear();
		}
		
		foreach (var btnSlot in playerDataModel.Select(playerModel => new CharSlotButton(playerModel.SlotNumber, MAX_FRAMES, playerModel)))
		{
			btnSlot.Connect(CharSlotButton.SignalName.SlotPressed, new Callable(this, nameof(OnSlotPressed)));
			_btnSlots.Add(btnSlot);
		}

		CallDeferred(nameof(AddSlots));
	}
	private void AddSlots()
	{
		foreach (var slot in _btnSlots)
		{
			_mainContainer.AddChild(slot);
		}
	}

	private void OnSlotPressed(bool pressed, CharSlotButton button)
	{
		GD.Print($"Slot {button.Name} pressed: {pressed}");
		
		// Desmarca todos os botões, exceto o que foi clicado
		if (!pressed)
		{
			_lblName.Text = $"Name: ";
			_lblLevel.Text = $"Level: {0.ToString()}";
			_lblVocation.Text = $"Vocation: None";
			
			_btnCreateChar.Disabled = true;
			_btnEnterGame.Disabled = true;
			return;
		}
		
		foreach (var btn in _btnSlots.Where(b => b != button))
		{
			btn.ButtonPressed = false;
		}

		if (button.Empty)
		{
			_lblName.Text = $"Name: ";
			_lblLevel.Text = $"Level: {0.ToString()}";
			_lblVocation.Text = $"Vocation: None";
			_btnCreateChar.Disabled = false;
		}
		else
		{
			var playerModel = button.GetPlayerModel();
			_lblName.Text = $"Name: {playerModel.Name}";
			_lblLevel.Text = $"Level: {playerModel.Level}";
			_lblVocation.Text = $"Vocation: {playerModel.Vocation.ToString()}";
        
			_btnCreateChar.Disabled = true;
			_btnEnterGame.Disabled = false;
		}
	}

	public void RegisterPacketHandlers()
	{
		ClientPacketProcessor.RegisterPacket<SCharacter>(HandleCharacter);

		return;
		
		void HandleCharacter(SCharacter packet)
		{
			GD.Print("Handling character packet");
			ResetState();
			AddCharacters(packet.PlayerDataModel);
		}
		
		void ResetState()
		{
			_lblName.Text = $"Name: ";
			_lblLevel.Text = $"Level: {0.ToString()}";
			_lblVocation.Text = $"Vocation: None";
			_btnCreateChar.Disabled = true;
			_btnEnterGame.Disabled = true;
			_createContainer.Hide();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		//ClientPacketProcessor.UnregisterPacket<SCharacter>();
	}
}
