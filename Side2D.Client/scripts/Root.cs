using Godot;
using Side2D.Models.Enum;
using Side2D.scripts.Host;

namespace Side2D.scripts;

public partial class Root : Node2D
{
	private TextureRect _splashScreen;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_splashScreen = GetNode<TextureRect>("texSplash");
		
		FadeInSplashScreen();
		return;
		
		void FadeInSplashScreen()
		{
			_splashScreen.Modulate = new Color(1, 1, 1, 0);
			
			var tween = CreateTween();
			tween.TweenProperty(_splashScreen, "modulate:a", 1f, 3f);
			tween.SetTrans(Tween.TransitionType.Expo);
			tween.TweenCallback(Callable.From(FadeOutSplashScreen));
			tween.Play();
		}
		
		void FadeOutSplashScreen()
		{
			var tween = CreateTween();
			tween.TweenProperty(_splashScreen, "modulate:a", 0f, 2f);
			//tween.SetParallel();
			//tween.TweenProperty(_splashScreen, "modulate:a", 0f, 3f);
			tween.SetTrans(Tween.TransitionType.Expo);
			tween.TweenCallback(Callable.From(StartMenu));
			tween.Play();
		}
		
		void StartMenu()
		{
			var clientManager = ApplicationHost.Instance.GetSingleton<ClientManager>();
			clientManager.Start();
			clientManager.ChangeClientState(ClientState.Menu);
		}
	}
}