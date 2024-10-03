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
    private LineEdit _txtName;
    private OptionButton _optGender;
    private OptionButton _optVocation;
    private TextureRect _texCreateSprite;
    private Button _btnCreate;

    public override void _Ready()
    {
        base._Ready();

        _mainContainer = GetNode<HBoxContainer>("%MainContainer");
        InitializeNodes();
        _connectSignals();
        RegisterPacketHandlers();
        _populateOptions();
        _updateSubmitButtonState();
    }

    private void InitializeNodes()
    {
        _lblName = GetNode<Label>("%lblName");
        _lblLevel = GetNode<Label>("%lblLevel");
        _lblVocation = GetNode<Label>("%lblVocation");
        _btnCreateChar = GetNode<Button>("%btnCreateChar");
        _btnEnterGame = GetNode<Button>("%btnEnterGame");
        _createContainer = GetNode<PanelContainer>("%CreateContainer");
        _txtName = GetNode<LineEdit>("%txtName");
        _optGender = GetNode<OptionButton>("%optGender");
        _optVocation = GetNode<OptionButton>("%optVocation");
        _texCreateSprite = GetNode<TextureRect>("%texCreateSprite");
        _btnCreate = GetNode<Button>("%btnCreate");
    }

    private void _populateOptions()
    {
        PopulateEnumOptions(_optGender, Enum.GetValues(typeof(Gender)).Cast<Gender>());
        PopulateEnumOptions(_optVocation, Enum.GetValues(typeof(Vocation)).Cast<Vocation>());

        for (var i = 0; i < _optVocation.GetItemCount(); i++)
        {
            if (i == 0) continue; // Pular "None"

            var vocation = Enum.GetValues(typeof(Vocation)).GetValue(i);
            
            var texture =
                GD.Load<Texture2D>(
                    $"res://resources/Vocation/{vocation}/sprites/weapon.png");

            _optVocation.SetItemIcon(i, texture);
        }
    }

    private void PopulateEnumOptions<T>(OptionButton optionButton, IEnumerable<T> enumValues) where T : Enum
    {
        foreach (var value in enumValues)
        {
            if (value.ToString() == "Count") continue; // Pular "Count"
            optionButton.AddItem(value.ToString(), Convert.ToInt32(value));
        }
    }

    private void _connectSignals()
    {
        _btnCreate.Connect(BaseButton.SignalName.Pressed, Callable.From(_onCreatePressed));
        _btnCreateChar.Connect(BaseButton.SignalName.Pressed, Callable.From(_onCreateCharPressed));
        _btnEnterGame.Connect(BaseButton.SignalName.Pressed, Callable.From(_onEnterGamePressed));
        
        _txtName.Connect(LineEdit.SignalName.TextChanged, Callable.From<string>(newText =>
            _UpdateValidationState(_txtName, newText.IsValidName())
        ));
        
        _optGender.Connect(OptionButton.SignalName.ItemSelected, Callable.From<int>(UpdateCreateSprite));
        _optVocation.Connect(OptionButton.SignalName.ItemSelected, Callable.From<int>(UpdateCreateSprite));
        
        _connectOptionValidation(_optGender, () => _optGender.Selected.IsValidGender());
        _connectOptionValidation(_optVocation, () => _optVocation.Selected.IsValidVocation());
    }

    private void _connectOptionValidation(OptionButton input, Func<bool> validationFunc)
    {
        input.Connect(OptionButton.SignalName.ItemSelected, Callable.From<int>(index =>
            _UpdateValidationState(input, validationFunc())
        ));
    }

    private void _UpdateValidationState(Control control, bool isValid)
    {
        control.Modulate = isValid ? new Color(0, 1, 0) : new Color(1, 0, 0);
        _updateSubmitButtonState();
    }

    private void _updateSubmitButtonState()
    {
        _btnCreate.Disabled = !(_optGender.Selected.IsValidGender() && _optVocation.Selected.IsValidVocation());
    }

    private void _onEnterGamePressed()
    {
        var clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;
        var slot = _btnSlots.First(a => a.ButtonPressed);
        
        var packet = new CPlayerUseCharacter
        {
            SlotNumber = slot.SlotNumber
        };

        clientPlayer.SendData(packet);
    }

    private void _onCreateCharPressed()
    {
        _createContainer.Show();
        base.Connect(Window.SignalName.CloseRequested, Callable.From(() =>
        {
            _closeCreateContainer();
            _slotsVisibility(true);
        }));
        _slotsVisibility(false);
        _btnCreateChar.Disabled = true;
    }

    private void _closeCreateContainer()
    {
        if (_createContainer.Visible)
        {
            _createContainer.Hide();
        }
    }

    private void _slotsVisibility(bool show)
    {
        foreach (var slot in _btnSlots)
        {
            if (show) slot.Show();
            else slot.Hide();
        }

        CanClose = show;
    }

    private void _onCreatePressed()
    {
        var alertManager = ApplicationHost.Instance.GetSingleton<AlertManager>();
        
        if (!_txtName.Text.IsValidName())
        {
            alertManager.AddAlert($"Invalid name: Name must have between {InputValidator.MinNameCaracteres} and {InputValidator.MaxNameCaracteres} characters");
            return;
        }

        if (!_optGender.Selected.IsValidGender())
        {
            alertManager.AddAlert("Invalid Gender");
            return;
        }
        
        if (!_optVocation.Selected.IsValidVocation())
        {
            alertManager.AddAlert("Invalid Vocation");
            return;
        }

        var clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;
        var packet = new CCreateCharacter
        {
            SlotNumber = _btnSlots.First(a => a.ButtonPressed).SlotNumber,
            Name = _txtName.Text,
            Gender = (Gender)_optGender.Selected,
            Vocation = (Vocation)_optVocation.Selected
        };
        
        clientPlayer.SendData(packet);
        _closeCreateContainer();
    }

    public void _addCharacters(List<PlayerDataModel> playerDataModel)
    {
        GD.Print($"Adding characters to slots quantity: {playerDataModel.Count}");
        
        foreach (var btnSlot in _btnSlots)
        {
            btnSlot.QueueFree();
        }
        _btnSlots.Clear();
        
        _btnSlots.AddRange(playerDataModel.Select(playerModel => 
        {
            var btnSlot = new CharSlotButton(playerModel.SlotNumber, MAX_FRAMES, playerModel);
            btnSlot.Connect(CharSlotButton.SignalName.SlotPressed, new Callable(this, nameof(_onSlotPressed)));
            return btnSlot;
        }));

        CallDeferred(nameof(_addSlots));
    }

    private void _addSlots()
    {
        foreach (var slot in _btnSlots)
        {
            _mainContainer.AddChild(slot);
        }
    }

    private void _onSlotPressed(bool pressed, CharSlotButton button)
    {
        GD.Print($"Slot {button.Name} pressed: {pressed}");

        if (!pressed)
        {
            ResetLabels();
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
            ResetLabels();
            _btnCreateChar.Disabled = false;
        }
        else
        {
            var playerModel = button.GetPlayerModel();
            UpdateLabels(playerModel);
            _btnCreateChar.Disabled = true;
            _btnEnterGame.Disabled = false;
        }
    }

    private void ResetLabels()
    {
        _lblName.Text = "Name: ";
        _lblLevel.Text = "Level: 0";
        _lblVocation.Text = "Vocation: None";
    }

    private void UpdateLabels(PlayerDataModel playerModel)
    {
        _lblName.Text = $"Name: {playerModel.Name}";
        _lblLevel.Text = $"Level: {playerModel.Level}";
        _lblVocation.Text = $"Vocation: {playerModel.Vocation}";
    }

    public void RegisterPacketHandlers()
    {
        ClientPacketProcessor.RegisterPacket<SCharacter>(HandleCharacter);
    }

    private void HandleCharacter(SCharacter packet)
    {
        GD.Print("Handling character packet");
        ResetState();
        if (packet.PlayerDataModel != null) 
            _addCharacters(packet.PlayerDataModel);
    }

    private void ResetState()
    {
        ResetLabels();
        _btnCreateChar.Disabled = true;
        _btnEnterGame.Disabled = true;
        _closeCreateContainer();
    }

    private void UpdateCreateSprite(int selected)
    {
        var vocation = (Vocation)_optVocation.Selected;
        var gender = (Gender)_optGender.Selected;

        if (vocation == Vocation.None)
        {
            _texCreateSprite.Texture = null;
            return;
        }

        if (gender == Gender.Undefined)
        {
            _texCreateSprite.Texture = null;
            return;
        }
        
        var texture = GD.Load<Texture2D>($"res://resources/Vocation/{vocation.ToString()}/sprites/{gender.ToString().ToLower()}/0.png");
        
        if (texture == null) return;
        
        _texCreateSprite.Texture = texture;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        // ClientPacketProcessor.UnregisterPacket<SCharacter>();
    }
}
