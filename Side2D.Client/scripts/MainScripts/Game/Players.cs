using System.Collections.Generic;
using Godot;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Server;
using Side2D.scripts.Network;
using ApplicationHost = Side2D.Host.ApplicationHost;

namespace Side2D.scripts.MainScripts.Game;

public partial class Players : Node, IPacketHandler
{
	private readonly List<Player> _players = [];
	
	private PackedScene _playerScene = GD.Load<PackedScene>("res://scenes/Game/Player.tscn");
	
	public Players()
	{
		Name = nameof(Players);
		RegisterPacketHandlers();
	}
	
	public void AddPlayer(bool isLocal, PlayerDataModel playerData, PlayerMoveModel playerMoveModel)
	{
		if (_playerScene.Instantiate() is not Player player) return;
		
		player.IsLocal = isLocal;
		
		player.PlayerDataModel = playerData;
		player.PlayerMoveModel = playerMoveModel;
		
		_players.Add(player);
		
		CallDeferred("add_child", player);
	}
	
	public void RemovePlayer(int index)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == index);
		
		if (player == null) return;
		
		player.QueueFree();
		
		_players.Remove(player);
	}
	
	public void PlayerMove(PlayerMoveModel playerMove)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == playerMove.Index);
		
		if (player == null) return;

		player.PlayerMoveModel.SetValues(playerMove);
		
		player.CallDeferred(nameof(player.UpdatePlayerMove));
	}

	public void RegisterPacketHandlers()
	{
		ClientPacketProcessor.RegisterPacket<SPlayerData>(ServerPlayerData);
		ClientPacketProcessor.RegisterPacket<SPlayerMove>(ServerPlayerMove);
		ClientPacketProcessor.RegisterPacket<SPlayerLeft>(ServerLeft);
	}
	public override void _ExitTree()
	{
		ClientPacketProcessor.UnregisterPacket<SPlayerData>();
		ClientPacketProcessor.UnregisterPacket<SPlayerMove>();
		ClientPacketProcessor.UnregisterPacket<SPlayerLeft>();
	}
	private void ServerPlayerData(SPlayerData obj)
	{
		var localIndex = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer.PlayerIndex;

		foreach (var playerDataModel in obj.PlayersDataModels)
		{
			var playerMoveModel = obj.PlayersMoveModels.Find(x => x.Index == playerDataModel.Index);
			var isLocal = playerDataModel.Index == localIndex;
			AddPlayer(isLocal, playerDataModel, playerMoveModel);
		}
	}
	private void ServerPlayerMove(SPlayerMove obj)
	{
		PlayerMove(obj.PlayerMoveModel);
	}
	private void ServerLeft(SPlayerLeft obj)
	{
		RemovePlayer(obj.Index);
	}
}