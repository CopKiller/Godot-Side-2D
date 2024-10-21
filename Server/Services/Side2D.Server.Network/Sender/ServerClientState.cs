using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    private void ServerClientState(int index, ClientState state)
    {
        players.TryGetValue(index, out var player);
        
        if (player == null) return;
        
        var obj = new SClientState()
        {
            Index = index,
            ClientState = state
        };
        SendDataTo(player.Peer, obj, DeliveryMethod.ReliableOrdered);
    }
}