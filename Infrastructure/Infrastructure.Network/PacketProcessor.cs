
using Core.Game.Models.Player;
using LiteNetLib;
using LiteNetLib.Utils;
using Infrastructure.Network.CustomDataSerializable;
using Infrastructure.Network.CustomDataSerializable.Extension;

namespace Infrastructure.Network
{
    public abstract class PacketProcessor : NetPacketProcessor
    {
        protected PacketProcessor() { }

        protected void RegisterCustomTypes()
        {
            // Register Types Of Serializations
            this.RegisterNestedType<PlayerMoveModel>();
            this.RegisterNestedType<PlayerDataModel>();
            this.RegisterNestedType<Vitals>(NetSerializerExtension.Put, NetSerializerExtension.GetVitals);
        }

        public virtual void SubscribePacket() { }

        public void Subscribe<T>(Action<T, NetPeer> onReceive) where T : class, new()
        {
            this.SubscribeReusable(onReceive);
            
            //Log.PrintError(onReceive.Method.Username + " Subscribed");
        }
    }
}
