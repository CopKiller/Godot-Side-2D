using Godot;
using Side2D.scripts.Network;

namespace Side2D.scripts.MainScripts.Game;

public partial class Game : Node2D, IPacketHandler
{

	public Game()
	{
		RegisterPacketHandlers();
	}
	

	// Implement IPacketHandler, for handling packets from the server, and registering handlers
	public void RegisterPacketHandlers()
	{
		//ClientPacketProcessor.Instance.SubscribeReusable<SMapData>(ReceiveMapData);
	}

	public override void _ExitTree()
	{
		//ClientPacketProcessor.Instance.RemoveSubscription<SMapData>();
	}
}