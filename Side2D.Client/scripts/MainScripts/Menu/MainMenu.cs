using Godot;
using LiteNetLib;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.scripts;
using Side2D.scripts.Host;

public partial class MainMenu : Control
{
	private MainMenuWindows _mainMenuWindows;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainMenuWindows = GetNode<MainMenuWindows>("MainMenuWindows");
		
		FadeInMainMenu();
		return;
		
		void FadeInMainMenu()
		{
			this.Modulate = new Color(1, 1, 1, 0);
			var tween = CreateTween();
			tween.TweenProperty(this, "modulate:a", 1f, 2f);
			
			tween.TweenCallback(Callable.From(() =>
			{
				_mainMenuWindows.StartMenu();
			}));
			
			tween.SetTrans(Tween.TransitionType.Linear);
			tween.Play();
		}
	}
}
