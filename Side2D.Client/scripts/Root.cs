using System.Collections.Generic;
using System.Linq;
using Godot;
using Side2D.Cryptography;
using Side2D.Models.Enum;
using ApplicationHost = Side2D.Host.ApplicationHost;

namespace Side2D.scripts;

public partial class Root : Node2D
{
	public bool IsDebugMode => false;
	
	private TextureRect _splashScreen;
	
	private List<Tween> _tweens = [];
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_splashScreen = GetNode<TextureRect>("texSplash");

		//CryptoManager.CreateDefault();
		
		//CryptoManager.Save(ConfigSection.User, ConfigKey.Password, "", true);
		
		if (IsDebugMode)
		{
			StartMenu();
			return;
		}
		
		FadeInSplashScreen();
		return;
		
		void FadeInSplashScreen()
		{
			_splashScreen.Modulate = new Color(1, 1, 1, 0);
			
			var tween = CreateTween();
			_tweens.Add(tween);
			tween.TweenProperty(_splashScreen, "modulate:a", 1f, 3f);
			tween.SetTrans(Tween.TransitionType.Sine);
			tween.TweenCallback(Callable.From(FadeOutSplashScreen));
			tween.Play();
		}
		
		void FadeOutSplashScreen()
		{
			var tween = CreateTween();
			_tweens.Add(tween);
			tween.TweenProperty(_splashScreen, "modulate:a", 0f, 2f);
			tween.SetTrans(Tween.TransitionType.Sine);
			tween.TweenCallback(Callable.From(StartMenu));
			tween.Play();
		}
	}
	
	private void StartMenu()
	{
		var clientManager = ApplicationHost.Instance.GetSingleton<ClientManager>();
		clientManager.ChangeClientState(ClientState.Menu);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is not (InputEventKey { Pressed: true, Keycode: Key.Escape } or InputEventMouse
		    {
			    ButtonMask: MouseButtonMask.Left
		    })) return;
		
		foreach (var tween in _tweens.ToList())
		{
			tween.Stop();
			_tweens.Remove(tween);
			tween.Dispose();
		}
	        
		StartMenu();

		return;
	}
}