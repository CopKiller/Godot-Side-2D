using System;
using Godot;

public partial class MainMenu : Control
{
	public Action Loaded;
	
	public MainMenuWindows MainMenuWindows { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MainMenuWindows = GetNode<MainMenuWindows>(nameof(MainMenuWindows));
		
		FadeInMainMenu();
		return;
		
		void FadeInMainMenu()
		{
			this.Modulate = new Color(1, 1, 1, 0);
			var tween = CreateTween();
			tween.TweenProperty(this, "modulate:a", 1f, 2f);
			
			tween.TweenCallback(Callable.From(() =>
			{
				MainMenuWindows.ShowMenuWindow();
				Loaded?.Invoke();
			}));
			
			tween.SetTrans(Tween.TransitionType.Linear);
			tween.Play();
		}
	}
}
