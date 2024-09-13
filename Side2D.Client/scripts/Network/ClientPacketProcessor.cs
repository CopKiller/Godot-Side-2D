using System;
using Godot;
using Side2D.Network;

namespace Side2D.scripts.Network
{
    public partial class ClientPacketProcessor: PacketProcessor
    {
        public ClientPacketProcessor() 
        {
            base.RegisterCustomTypes();
        }
    }
}
