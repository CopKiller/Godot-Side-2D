using System;
using Godot;

namespace Side2D.scripts.Controls;

public partial class BaseWindow : Window
{
    [Signal]
    public delegate void OnCloseEventHandler();
    
    [Signal]
    public delegate void HandleCloseEventHandler();
    
    
    // Can Close this window?
    protected bool CanClose { get; set; } = true;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignSignals();
    }

    private void AssignSignals()
    {
        this.Connect(Window.SignalName.CloseRequested, Callable.From(CloseWindow));
    }

    public void CloseWindow()
    {
        EmitSignal(SignalName.HandleClose); // --> Emited First, for close others childrens
        
        if (!CanClose) return;
        this.Hide();
        EmitSignal(SignalName.OnClose);
    }

    public void ShowWindow()
    {
        this.Show();
    }
    
    public void ShowWindow(Action? onClose)
    {
        if (onClose != null) this.Connect(SignalName.OnClose, Callable.From(onClose));
        ShowWindow();
    }
}