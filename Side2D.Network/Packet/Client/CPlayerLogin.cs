
using LiteNetLib;

namespace Side2D.Network.Packet.Client
{
    public class CPlayerLogin
    {
        public string Username { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;
    }
}
