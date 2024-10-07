

using LiteNetLib.Utils;
using Side2D.Models.Player;
using Side2D.Network.CustomDataSerializable.Extension;

namespace Side2D.Network.Packet.Server
{
    public class SPlayerUpdateVitals
    {
        public int Index { get; set; }
        public Vitals? Vitals { get; set; }
        
        public static SPlayerUpdateVitals Create(int index, Vitals? vitals)
        {
            return new SPlayerUpdateVitals
            {
                Index = index,
                Vitals = vitals
            };
        }
    }
}
