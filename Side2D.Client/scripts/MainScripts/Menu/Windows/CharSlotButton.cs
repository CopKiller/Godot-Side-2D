using System.Collections.Generic;
using Godot;
using Side2D.Models;
using Side2D.Network.CustomDataSerializable;

namespace Side2D.scripts.MainScripts.Menu.Windows;

public partial class CharSlotButton(int slotNumber, int countFrames, PlayerDataModel playerDataModel) : Button
{
    // Seção específica para declaração de diretórios de arquivos.
    private string _dirVocation =>
        $"res://resources/Vocation/{playerDataModel.Vocation.ToString()}/sprites/{playerDataModel.Gender.ToString().ToLower()}/";
    
    [Signal]
    public delegate void SlotPressedEventHandler(bool pressed, CharSlotButton btnSlot);
    
    private List<Texture2D> _slotTextures = [];
    
    public int SlotNumber => slotNumber;
    
    public bool Empty => playerDataModel.Name == string.Empty;

    public override void _Ready()
    {
        this.Toggled += PressedSlot;
        
        this.Name = $"btnSlot{slotNumber}";
        
        this.ToggleMode = true;
        this.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        this.SizeFlagsVertical = SizeFlags.ExpandFill;

        this.IconAlignment = HorizontalAlignment.Center;
        this.VerticalIconAlignment = VerticalAlignment.Bottom;
        
        Text = Empty ? $"Slot {slotNumber} (Empty)" : $"Slot {slotNumber} (Used)";

        if (Empty) return;
        
        for (var i = 1; i <= countFrames; i++)
        {
            _slotTextures.Add(GD.Load<Texture2D>($"{_dirVocation + i}.png"));
        }
        
        this.Icon = _slotTextures[0];
        
        return;
    }
    
    public override void _Process(double delta)
    {
        if (Empty) return;
        
        var frame = (int) (Time.GetTicksMsec() / 1000) % _slotTextures.Count;
        this.Icon = _slotTextures[frame];
    }
    
    public PlayerDataModel GetPlayerModel()
    {
        return playerDataModel;
    }
    
    private void PressedSlot(bool pressed)
    {
        EmitSignal(SignalName.SlotPressed, pressed, this);
    }
    
    public override void _ExitTree()
    {
        this.Toggled -= PressedSlot;
    }
    
    public new void QueueFree()
    {
        CallDeferred("queue_free");
    }
}