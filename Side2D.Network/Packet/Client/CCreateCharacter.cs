
using LiteNetLib;
using Side2D.Models.Enum;
using Side2D.Network.CustomDataSerializable;

namespace Side2D.Network.Packet.Client
{
    public class CCreateCharacter
    {
        public int SlotNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public Vocation Vocation { get; set; }
    }
}
