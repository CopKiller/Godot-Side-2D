using System;
using Godot;

namespace Side2D.scripts.Controls;

public partial class BaseWindow : Window
{
    // Open other window on close?
    private Action _onClose;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignSignals();
    }

    private void AssignSignals()
    {
        this.CloseRequested += CloseWindow;
    }

    public void CloseWindow()
    {
        this.Hide();
        _onClose?.Invoke();
    }

    public void ShowWindow()
    {
        this.Show();
    }
    
    public void ShowWindow(Action onClose)
    {
        _onClose = onClose;
        ShowWindow();
    }
}