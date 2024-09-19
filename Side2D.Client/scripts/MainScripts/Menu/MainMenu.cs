using Godot;
using LiteNetLib;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.scripts;
using Side2D.scripts.Host;

public partial class MainMenu : Control
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		FadeInMainMenu();
		return;
		
		void FadeInMainMenu()
		{
			this.Modulate = new Color(1, 1, 1, 0);
			var tween = CreateTween();
			tween.TweenProperty(this, "modulate:a", 1f, 2f);
			tween.SetTrans(Tween.TransitionType.Linear);
			//tween.TweenCallback(Callable.From(FadeOutSplashScreen));
			tween.Play();
		}
	}
	
	
	/*_button = GetNode<Button>("%Button");
		_button.Connect("pressed", Callable.From(() =>
		{
			var clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;

			var packet = new CPlayerLogin();

			/*var packet = new CAccountRegister()
			{
				AccountRegisterModel = new AccountRegisterModel()
				{
					Username = "test",
					Password = "password123",
					Email = "Rafuxo@gmail.com"
				}
			};#1#

			clientPlayer.SendData(packet, DeliveryMethod.ReliableOrdered);
			QueueFree();
		}));*/
}
