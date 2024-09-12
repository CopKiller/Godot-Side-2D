using System.Collections.Generic;
using Godot;
using Side2D.Network.CustomDataSerializable;

namespace Side2D.scripts;

public partial class Players : Node
{
	private readonly List<Player> _players = [];
	
	private PackedScene _playerScene = GD.Load<PackedScene>("res://scenes/Player.tscn");
	
	public void AddPlayer(bool isLocal, PlayerDataModel playerData, PlayerMoveModel playerMove)
	{
		if (_playerScene.Instantiate() is not Player player) return;
		
		player.IsLocal = isLocal;
		
		player.PlayerDataModel = playerData;
		player.PlayerMoveModel = playerMove;
		
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
		
		player.PlayerMoveModel = playerMove;
		
		player.CallDeferred(nameof(player.UpdatePlayer));
	}
}