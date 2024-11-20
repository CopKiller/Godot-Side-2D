using LiteNetLib;

namespace Side2D.Server.NetworkService.Interfaces;

public interface IServerPacketProcessor
{
    void Initialize();
    
    void SubscribePacket();
    
    void SendDataToAllBut<T>(NetPeer excludePeer, T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new();
    
    void SendDataToAll<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new();
    
    void SendDataTo<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new();
}