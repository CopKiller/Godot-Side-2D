

using LiteNetLib.Utils;
using Side2D.Models;
using Side2D.Network.CustomDataSerializable;

namespace Side2D.Network.Packet.Server
{
    public class SCharacter
    {
        public List<PlayerDataModel>? PlayerDataModel { get; set; }
    }
}
