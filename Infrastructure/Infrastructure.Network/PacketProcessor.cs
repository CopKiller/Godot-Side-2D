
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
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
            this.RegisterNestedType<PlayerDataModel>();
            this.RegisterNestedType<Vitals>(NetSerializerExtension.Put, NetSerializerExtension.GetVitals);
            this.RegisterNestedType<VectorTwo>(NetSerializerExtension.Put, NetSerializerExtension.GetVector2);
            this.RegisterNestedType<Position>(NetSerializerExtension.Put, NetSerializerExtension.GetPosition);
        }
    }
}
