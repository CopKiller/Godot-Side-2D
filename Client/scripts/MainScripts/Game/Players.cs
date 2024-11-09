using System.Collections.Generic;
using Core.Game.Models.Player;
using Godot;
using Infrastructure.Logger;
using Infrastructure.Network.CustomDataSerializable;
using Infrastructure.Network.Packet.Server;
using Side2D.Host;
using Side2D.scripts.Controls.CustomLabels;
using Side2D.scripts.Controls.Fonts;
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
	
	private void PlayerMove(SUpdateBody obj)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == obj.Index);
		
		if (player == null) return;

		player.PlayerDataModel.Position.SetValues(obj.Position);
		player.PlayerDataModel.Position.Velocity.SetValues(obj.Velocity);
		//player.PlayerDataModel.Position.Rotation = obj.Rotation;
		
		player.CallDeferred(nameof(player.UpdatePlayerMove));
	}
	
	private void PlayerImpact(SUpdateKnockback impact)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == impact.Index);
		
		player?.CallDeferred(nameof(player.UpdateImpact), new Vector2(impact.Position.X, impact.Position.Y));
	}
	
	private void PlayerAttack(SPlayerAttack attack)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == attack.Index);

		player?.CallDeferred(nameof(player.Attack), true, (byte)attack.AttackType);
	}
	
	public void PlayerNotifyVitals(int index, double value)
	{
		var player = _players.Find(x => x.PlayerDataModel.Index == index);
		
		if (player == null) return;

		var font = FontManager.LoadFont("Damage Bold.otf");
		
		var customLabelIncrement = value > 0 ? CustomLabelIncrement.Increase : CustomLabelIncrement.Decrease;
		var customLabelColor = value > 0 ? CustomLabelColor.Green : CustomLabelColor.Red;
		
		var customLabel = new CustomLabel(value, new Vector2(0, 0), player, font,
			CustomLabelSize.ExtraLarge, CustomLabelIncrease.None, customLabelColor, customLabelIncrement);
		
		var tween = customLabel.Tween;

		tween?.SetParallel();
		tween?.TweenProperty(customLabel, "position:y", -64, 1.5f);
		tween?.TweenProperty(customLabel, "modulate:a", 0, 1.5f);
		
		tween?.Connect("finished", Callable.From(() =>
		{
			customLabel.QueueFree();
		}));
		
		player.AddChild(customLabel);
	}

	public void RegisterPacketHandlers()
	{
		ClientPacketProcessor.RegisterPacket<SPlayerData>(ServerPlayerData);
		ClientPacketProcessor.RegisterPacket<SUpdateBody>(ServerPlayerMove);
		ClientPacketProcessor.RegisterPacket<SPlayerLeft>(ServerLeft);
		ClientPacketProcessor.RegisterPacket<SPlayerAttack>(ServerPlayerAttack);
		ClientPacketProcessor.RegisterPacket<SUpdateKnockback>(ServerPlayerImpact);
		ClientPacketProcessor.RegisterPacket<SVitalsNotification>(ServerVitalsNotification);
	}
	public override void _ExitTree()
	{
		ClientPacketProcessor.UnregisterPacket<SPlayerData>();
		ClientPacketProcessor.UnregisterPacket<SUpdateBody>();
		ClientPacketProcessor.UnregisterPacket<SPlayerLeft>();
		ClientPacketProcessor.UnregisterPacket<SPlayerAttack>();
		ClientPacketProcessor.UnregisterPacket<SUpdateKnockback>();
		ClientPacketProcessor.UnregisterPacket<SVitalsNotification>();
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
	private void ServerPlayerMove(SUpdateBody obj)
	{
		GD.Print(obj.Position);
		
		PlayerMove(obj);
	}

	private void ServerPlayerAttack(SPlayerAttack attack)
	{
		PlayerAttack(attack);
	}
	
	private void ServerLeft(SPlayerLeft obj)
	{
		RemovePlayer(obj.Index);
	}
	
	private void ServerPlayerImpact(SUpdateKnockback obj)
	{
		PlayerImpact(obj);
	}
	
	private void ServerVitalsNotification(SVitalsNotification packet)
	{
		CallDeferred(nameof(PlayerNotifyVitals), packet.Index, packet.Value);
	}
}