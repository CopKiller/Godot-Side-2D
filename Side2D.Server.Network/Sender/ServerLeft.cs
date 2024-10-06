
using LiteNetLib;
using Side2D.Logger;
using Side2D.Models.Enum;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ServerLeft(NetPeer netPeer, SPlayerLeft sPlayerLeft, bool isDisconnect = true)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;
            
            SendDataToAllBut(netPeer, sPlayerLeft, ClientState.Game, DeliveryMethod.ReliableUnordered);

            if (!isDisconnect) return;
                
            ServerNetworkService.Players?.Remove(netPeer.Id);
                
            player.Peer.Disconnect();
        }
    }
}
