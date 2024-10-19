using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;
using Infrastructure.Network.Packet.Client;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ClientPlayerMove(CPlayerMove obj, NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player == null) return;
            
        if (player.TempPlayer.ClientState != ClientState.Game) return;
        
        if ( !player.PhysicPlayer.MovePlayer(obj.PlayerMoveModel.Position))
        {
            ServerAlert(netPeer, "Invalid move!");
            return;
        }
        
        var playerMoveModel = player.PlayerMoveModel;
        playerMoveModel.IsMoving = obj.PlayerMoveModel.IsMoving;
        playerMoveModel.Velocity = obj.PlayerMoveModel.Velocity;
        player.PlayerMoveModel = playerMoveModel;
        
        player.PlayerMoveModel.Position?.NotifyPositionChanged?.Invoke();
    }
}