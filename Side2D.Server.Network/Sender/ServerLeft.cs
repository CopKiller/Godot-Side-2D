
using LiteNetLib;
using Side2D.Logger;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ServerLeft(NetPeer netPeer, SPlayerLeft sPlayerLeft)
        {
            ServerNetworkService.Players?.Remove(netPeer.Id);
            SendDataToAllBut(netPeer, sPlayerLeft, DeliveryMethod.ReliableUnordered);
        }
    }
}
