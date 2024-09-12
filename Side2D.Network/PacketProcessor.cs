
using System.Numerics;
using LiteNetLib;
using LiteNetLib.Utils;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.CustomDataSerializable.Extension;

namespace Side2D.Network
{
    public class PacketProcessor : NetPacketProcessor
    {
        protected PacketProcessor() { }

        protected void RegisterCustomTypes()
        {
            // Register Types Of Serializations
            this.RegisterNestedType<PlayerMoveModel>();
            this.RegisterNestedType<PlayerDataModel>();
        }

        public virtual void SubscribePacket() { }

        public void Subscribe<T>(Action<T, NetPeer> onReceive) where T : class, new()
        {
            this.SubscribeReusable(onReceive);
            
            //Log.PrintError(onReceive.Method.Name + " Subscribed");
        }
        
        public virtual void SendDataTo<T>(NetPeer playerPeer, T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
        {
            Send(playerPeer, packet, deliveryMethod);
        }
    }
}
