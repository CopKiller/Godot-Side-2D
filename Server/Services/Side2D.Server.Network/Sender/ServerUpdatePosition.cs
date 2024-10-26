using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Infrastructure.Logger;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ServerUpdatePosition(int index, EntityType type, bool includeSelf = false)
    {
        
        if (type != EntityType.Player) return;
        
        players.TryGetValue(index, out var player);
        
        if (player == null) return;
        
        var packet = SPlayerMove.Create(player.PlayerDataModel.Position);
        
        if (includeSelf)
            SendDataToAll(packet, ClientState.Game, DeliveryMethod.Sequenced);
        else
            SendDataToAllBut(player.Peer, packet, ClientState.Game, DeliveryMethod.Sequenced);
    }
}