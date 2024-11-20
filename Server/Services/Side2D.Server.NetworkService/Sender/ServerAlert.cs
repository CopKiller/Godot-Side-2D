using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.NetworkService;

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