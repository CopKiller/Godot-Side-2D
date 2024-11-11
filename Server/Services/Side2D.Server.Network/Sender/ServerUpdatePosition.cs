using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Infrastructure.Logger;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;
using Microsoft.Xna.Framework;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ServerUpdateBody(int index, EntityType type, Vector2 position, Vector2 velocity, 
        int rotation, bool includeSelf = false)
    {
        players.TryGetValue(index, out var player);
        
        if (player == null) return;
        
        var packet = SUpdateBody.Create(index, type, position, velocity, rotation);
        
        if (includeSelf)
            SendDataToAll(packet, ClientState.Game, DeliveryMethod.Sequenced);
        else
            SendDataToAllBut(player.Peer, packet, ClientState.Game, DeliveryMethod.Sequenced);
    }
}