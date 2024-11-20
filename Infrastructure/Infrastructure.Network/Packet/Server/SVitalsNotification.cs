

using Core.Game.Models.Enum;
using LiteNetLib.Utils;

namespace Infrastructure.Network.Packet.Server
{
    public class SVitalsNotification
    {
        public int Index { get; set; }
        public VitalsType VitalsType { get; set; }
        public double Value { get; set; }
        
        public static SVitalsNotification Create(int index, VitalsType vitalsType, double value)
        {
            return new SVitalsNotification
            {
                Index = index,
                VitalsType = vitalsType,
                Value = value
            };
        }
    }
}
