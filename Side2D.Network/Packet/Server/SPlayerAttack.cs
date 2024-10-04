using LiteNetLib.Utils;
using Side2D.Models.Enum;
using Side2D.Network.CustomDataSerializable;

namespace Side2D.Network.Packet.Server
{
    public class SPlayerAttack
    {
        public int Index { get; set; }
        public AttackType AttackType { get; set; }
    }

}
