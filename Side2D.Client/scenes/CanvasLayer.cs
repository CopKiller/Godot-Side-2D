using Godot;
using LiteNetLib;
using Side2D.Network.Packet.Client;
using Side2D.scripts;

public partial class CanvasLayer : Godot.CanvasLayer
{
	private Button _button;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_button = GetNode<Button>("%Button");
		_button.Connect("pressed", Callable.From(() =>
		{
			var loginPacket = new CPlayerLogin();
			ClientManager.Instance.ClientPlayer.SendData(loginPacket, DeliveryMethod.ReliableOrdered);
			QueueFree();
		}));
	}
}
