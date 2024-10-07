﻿
using System.Numerics;
using LiteNetLib;
using LiteNetLib.Utils;
using Side2D.Models.Player;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.CustomDataSerializable.Extension;

namespace Side2D.Network
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
