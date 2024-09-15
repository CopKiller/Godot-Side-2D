
using LiteNetLib;
using Side2D.Network.CustomDataSerializable;

namespace Side2D.Network.Packet.Client
{
    public class CAccountRegister
    {
        public AccountRegisterModel AccountRegisterModel { get; set; }
    }
}
