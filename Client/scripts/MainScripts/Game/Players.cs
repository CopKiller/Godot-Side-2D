using System.Collections.Generic;
using Core.Game.Models.Player;
using Godot;
using Infrastructure.Network.CustomDataSerializable;
using Infrastructure.Network.Packet.Server;
using Side2D.Host;
using Side2D.scripts.Network;

namespace Side2D.scripts.MainScripts.Game;

public partial class Players : Node, IPacketHandler
{
	// Seção específica de diretórios de arquivos.
	private const string PLAYER_SPRITE_PATH = "res://scenes/Game/Player.tscn";
	
	private readonly List<Player> _players = [];
	
	private PackedScene _playerScene = GD.Load<PackedScene>(PLAYER_SPRITE_PATH);
	
	public Players()
	{
		Name = nameof(Players);
		RegisterPacketHandlers();
	}
	
	private void AddPlayer(bool isLocal, PlayerDataModel playerData)
	{
		if (_playerScene.Instantiate() is not Player player) return;
		
		player.IsLocal = isLocal;
		
		player.PlayerDataModel = playerData;
		
		_players.Add(player);
		
		CallDeferred("add_child", player);
	}
	
	private void RemovePlayer(int index)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == index);
		
		if (player == null) return;
		
		player.QueueFree();
		
		_players.Remove(player);
	}
	
	private void PlayerMove(Position playerMove)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == playerMove.Index);
		
		if (player == null) return;

		player.PlayerDataModel.Position.SetValues(playerMove);
		
		player.CallDeferred(nameof(player.UpdatePlayerMove));
	}
	
	private void PlayerImpact(SPlayerImpact impact)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == impact.Index);
		
		player?.CallDeferred(nameof(player.UpdateImpact), new Vector2(impact.ImpactVelocity.X, impact.ImpactVelocity.Y));
	}
	
	private void PlayerAttack(SPlayerAttack attack)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == attack.Index);

		player?.CallDeferred(nameof(player.Attack), true, (byte)attack.AttackType);
	}

	public void RegisterPacketHandlers()
	{
		ClientPacketProcessor.RegisterPacket<SPlayerData>(ServerPlayerData);
		ClientPacketProcessor.RegisterPacket<SPlayerMove>(ServerPlayerMove);
		ClientPacketProcessor.RegisterPacket<SPlayerLeft>(ServerLeft);
		ClientPacketProcessor.RegisterPacket<SPlayerAttack>(ServerPlayerAttack);
		ClientPacketProcessor.RegisterPacket<SPlayerImpact>(ServerPlayerImpact);
	}
	public override void _ExitTree()
	{
		ClientPacketProcessor.UnregisterPacket<SPlayerData>();
		ClientPacketProcessor.UnregisterPacket<SPlayerMove>();
		ClientPacketProcessor.UnregisterPacket<SPlayerLeft>();
		ClientPacketProcessor.UnregisterPacket<SPlayerAttack>();
		ClientPacketProcessor.UnregisterPacket<SPlayerImpact>();
	}
	private void ServerPlayerData(SPlayerData obj)
	{
		var localIndex = ApplicationHost.Instance.GetSingleton<ClientManager>().ClientPlayer.PlayerIndex;

		foreach (var playerDataModel in obj.PlayersDataModels)
		{
			var isLocal = playerDataModel.Index == localIndex;
			AddPlayer(isLocal, playerDataModel);
		}
	}
	private void ServerPlayerMove(SPlayerMove obj)
	{
		GD.Print(obj.Position);
		
		PlayerMove(obj.Position);
	}

	private void ServerPlayerAttack(SPlayerAttack attack)
	{
		PlayerAttack(attack);
	}
	
	private void ServerLeft(SPlayerLeft obj)
	{
		RemovePlayer(obj.Index);
	}
	
	private void ServerPlayerImpact(SPlayerImpact obj)
	{
		PlayerImpact(obj);
	}
}