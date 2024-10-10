using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ServerLeft(NetPeer netPeer, SPlayerLeft sPlayerLeft, bool isDisconnect = true)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player == null) return;
            
        SendDataToAllBut(netPeer, sPlayerLeft, ClientState.Game, DeliveryMethod.ReliableUnordered);

        if (!isDisconnect) return;
                
        players.Remove(netPeer.Id);
                
        player.Peer.Disconnect();
    }
}