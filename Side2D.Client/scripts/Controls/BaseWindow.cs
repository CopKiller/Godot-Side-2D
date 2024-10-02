using System;
using Godot;

namespace Side2D.scripts.Controls;

public partial class BaseWindow : Window
{
    // Open other window on close?
    private Action? _onClose;

    private Action? _closeChildComponent;
    
    // Can Close this window?
    protected bool CanClose { get; set; } = true;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignSignals();
    }

    private void AssignSignals()
    {
        this.CloseRequested += CloseWindow;
    }
    
    private void UnassignSignals()
    {
        this.CloseRequested -= CloseWindow;
        _onClose = null;
        _closeChildComponent = null;
    }

    public void CloseWindow()
    {
        if (_closeChildComponent != null)
        {
            _closeChildComponent.Invoke();
            _closeChildComponent = null;
        }
        else
        {
            if (!CanClose) return;
            this.Hide();
            _onClose?.Invoke();
        }
    }

    public void ShowWindow()
    {
        this.Show();
    }
    
    public void ShowWindow(Action? onClose)
    {
        _onClose = onClose;
        ShowWindow();
    }

    protected void AddActionCloseChildComponent(Action action)
    {
        _onClose = _closeChildComponent;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        
        UnassignSignals();
    }
}