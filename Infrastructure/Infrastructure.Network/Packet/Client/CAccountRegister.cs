
using LiteNetLib;
using Infrastructure.Network.CustomDataSerializable;

namespace Infrastructure.Network.Packet.Client
{
    public class CAccountRegister
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
