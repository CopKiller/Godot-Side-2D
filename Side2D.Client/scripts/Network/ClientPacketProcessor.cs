using System;
using Godot;
using Side2D.Network;

namespace Side2D.scripts.Network
{
    public partial class ClientPacketProcessor: PacketProcessor
    {
        private static ClientPacketProcessor _instance;
        public ClientPacketProcessor()
        {
            _instance = this;
            base.RegisterCustomTypes();
        }
        
        public static void RegisterPacket<T>(Action<T> onReceive) where T : class, new()
        {
            _instance.SubscribeReusable(onReceive);
        }
        
        public static void UnregisterPacket<T>()
        {
            _instance.RemoveSubscription<T>();
        }
    }
}
