
using LiteNetLib;
using Side2D.Logger;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ServerAlert(NetPeer netPeer, string alertMsg)
        {
            var packet = new SAlertMsg()
            {
                Message = alertMsg
            };
            
            SendDataTo(netPeer, packet, DeliveryMethod.ReliableUnordered);
        }
    }
}
