using System.Collections.Generic;
using Godot;
using LiteNetLib;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;
using Side2D.scripts.Network;

namespace Side2D.scripts;

public partial class Map : Node2D
{
	private Players _players { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_players = GetNode<Players>(nameof(Players));
	}
}