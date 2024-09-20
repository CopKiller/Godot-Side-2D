
using LiteNetLib;
using Side2D.Network.CustomDataSerializable;

namespace Side2D.Network.Packet.Client
{
    public class CAccountRegister
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
