using Core.Game.Models.Enum;
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
            
        if (player.TempPlayer.Move == null) return;
            
        if ( ! player.TempPlayer.Move.CanMove(obj.PlayerMoveModel.Position))
        {
            ServerAlert(netPeer, "Invalid move!");
            return;
        }

        var receivedMove = obj.PlayerMoveModel;
        receivedMove.Index = netPeer.Id;    
            
        player.PlayerMoveModel = receivedMove;
            
        var packet = SPlayerMove.Create(player.PlayerMoveModel);

        SendDataToAllBut(netPeer, packet, ClientState.Game, DeliveryMethod.ReliableSequenced);
    }
}