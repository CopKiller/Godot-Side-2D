using Godot;
using LiteNetLib;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.scripts;
using Side2D.scripts.Host;

public partial class MainMenu : Control
{
	private Button _button;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_button = GetNode<Button>("%Button");
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
			};*/
			
			clientPlayer.SendData(packet, DeliveryMethod.ReliableOrdered);
			QueueFree();
		}));
	}
}
