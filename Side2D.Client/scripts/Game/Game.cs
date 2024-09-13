using Godot;
using System;
using Side2D.Network.Packet.Server;
using Side2D.scripts.Network;

public partial class Game : Node2D, IPacketHandler
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RegisterPacketHandlers();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
	
	//public void ReceiveMapData(SMapData packet)
}
