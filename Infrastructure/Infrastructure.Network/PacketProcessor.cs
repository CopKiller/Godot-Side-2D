
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
            this.RegisterNestedType<PlayerMoveModel>();
            this.RegisterNestedType<PlayerDataModel>();
            this.RegisterNestedType<Vitals>(NetSerializerExtension.Put, NetSerializerExtension.GetVitals);
            this.RegisterNestedType<Vector2C>(NetSerializerExtension.Put, NetSerializerExtension.GetVector2);
        }
    }
}
