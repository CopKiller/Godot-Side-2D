using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Infrastructure.Logger;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ServerUpdatePosition(int index, bool includeSelf = false)
    {
        players.TryGetValue(index, out var player);
        
        if (player == null) return;
        
        var packet = SPlayerMove.Create(player.PlayerMoveModel);
        
        if (includeSelf)
            SendDataToAll(packet, ClientState.Game, DeliveryMethod.Sequenced);
        else
            SendDataToAllBut(player.Peer, packet, ClientState.Game, DeliveryMethod.Sequenced);
        
        Log.Print($"Player {index} moved to {player.PlayerMoveModel.Position} Position Id: {player.PlayerMoveModel.Position.Id}");
    }
}