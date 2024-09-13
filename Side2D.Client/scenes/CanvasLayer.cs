using Godot;
using LiteNetLib;
using Side2D.Network.Packet.Client;
using Side2D.scripts;
using Side2D.scripts.Host;
using Side2D.scripts.Network;

public partial class CanvasLayer : Godot.CanvasLayer
{
	private Button _button;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_button = GetNode<Button>("%Button");
		_button.Connect("pressed", Callable.From(() =>
		{
			var clientPlayer = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer;
			
			var loginPacket = new CPlayerLogin();
			
			clientPlayer.SendData(loginPacket, DeliveryMethod.ReliableOrdered);
			QueueFree();
		}));
	}
}
