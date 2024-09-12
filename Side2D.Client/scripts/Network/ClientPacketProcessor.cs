using System;
using Godot;
using Side2D.Network;

namespace Side2D.scripts.Network
{
    public partial class ClientPacketProcessor: PacketProcessor
    {
        // Singleton
        public static ClientPacketProcessor Instance { get; set; }
        
        public ClientPacketProcessor() 
        {
            Instance = this;
            base.RegisterCustomTypes();
        }
    }
}
