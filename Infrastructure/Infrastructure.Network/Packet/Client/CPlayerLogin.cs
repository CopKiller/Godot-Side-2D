
using LiteNetLib;

namespace Infrastructure.Network.Packet.Client
{
    public class CPlayerLogin
    {
        public string Username { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;
        
        public static CPlayerLogin Create(string username, string password)
        {
            return new CPlayerLogin
            {
                Username = username,
                Password = password
            };
        }
    }
}
