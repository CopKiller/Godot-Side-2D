using System.Collections.Generic;
using Godot;
using LiteNetLib;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.scripts;

public partial class Map : Node2D
{
	public static Map Instance { get; private set; }
	
	private ClientManager _clientManager;
	public Players _players { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		_players = GetNode<Players>(nameof(Players));
		
		_clientManager = new ClientManager();
		_clientManager.Start();
	}

	public void AddPlayers(SPlayerData playerDataModels)
	{
		foreach (var playerDataModel in playerDataModels.PlayersDataModels)
		{
			var playerMoveModel = playerDataModels.PlayersMoveModels.Find(x => x.Index == playerDataModel.Index);
			var isLocal = playerDataModel.Index == _clientManager.ClientPlayer.PlayerIndex;
			_players.AddPlayer(isLocal, playerDataModel, playerMoveModel);
		}
	}
}