using Core.Game.Models.Enum;
using Godot;
using Side2D.Host;

namespace Side2D.scripts;

public partial class Root : Node2D
{
    public bool IsDebugMode => false;

    private TextureRect _splashScreen;
    private Label _lblInfo;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _splashScreen = GetNode<TextureRect>("texSplash");
        _lblInfo = GetNode<Label>("lblInfo");
        
        PulseLabel();

        if (IsDebugMode)
        {
            StartMenu();    
        }
        else
        {
            FadeInSplashScreen();
        }
    }
    
    private void PulseLabel()
    {
        var tween = CreateTween();
        
        tween.TweenProperty(_lblInfo, "scale", new Vector2(1.2f, 1.2f), 1f)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);
        
        tween.TweenCallback(Callable.From(() => 
        {
            var shrinkTween = CreateTween();
            shrinkTween.TweenProperty(_lblInfo, "scale", new Vector2(1f, 1f), 1f)
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.InOut);
            
            shrinkTween.TweenCallback(Callable.From(PulseLabel));
        }));
    
        tween.Play();
    }

    private void FadeInSplashScreen()
    {
        _splashScreen.Modulate = new Color(1, 1, 1, 0);
        
        var tween = CreateTween();
        tween.TweenProperty(_splashScreen, "modulate:a", 1f, 3f);
        tween.SetTrans(Tween.TransitionType.Sine);
        tween.TweenCallback(Callable.From(FadeOutSplashScreen));
        tween.Play();
    }

    private void FadeOutSplashScreen()
    {
        var tween = CreateTween();
        tween.TweenProperty(_splashScreen, "modulate:a", 0f, 2f);
        tween.SetTrans(Tween.TransitionType.Sine);
        tween.TweenCallback(Callable.From(StartMenu));
        tween.Play();
    }

    private void StartMenu()
    {
        var clientManager = ApplicationHost.Instance.GetSingleton<ClientManager>();
        clientManager.ChangeClientState(ClientState.Menu);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not (InputEventKey { Pressed: true, Keycode: Key.Escape } or InputEventMouse { ButtonMask: MouseButtonMask.Left }))
            return;
        
        StartMenu();
    }
}