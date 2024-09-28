
using LiteNetLib;
using Side2D.Logger;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ServerLeft(NetPeer netPeer, SPlayerLeft sPlayerLeft, bool isDisconnect = true)
        {
            if (isDisconnect)
            {
                var player = ServerNetworkService.Players?[netPeer.Id];
                
                if (player == null) return;
                
                ServerNetworkService.Players?.Remove(netPeer.Id);
            }
            
            SendDataToAllBut(netPeer, sPlayerLeft, DeliveryMethod.ReliableUnordered);
        }
    }
}
