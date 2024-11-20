using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.NetworkService;

public partial class ServerPacketProcessor
{
    public void ServerUpdateKnockback(int index, EntityType type, bool includeSelf, VectorTwo newPosition)
    {
        
        if (type != EntityType.Player) return;
        
        players.TryGetValue(index, out var player);
            
        if (player == null) return;
        
        if (player.TempPlayer.ClientState != ClientState.Game) return;
        
        var packet = SUpdateKnockback.Create(index, newPosition);
        
        if (includeSelf)
            SendDataToAll(packet, ClientState.Game, DeliveryMethod.ReliableSequenced);
        else
            SendDataToAllBut(player.Peer, packet, ClientState.Game, DeliveryMethod.ReliableSequenced);
    }
}