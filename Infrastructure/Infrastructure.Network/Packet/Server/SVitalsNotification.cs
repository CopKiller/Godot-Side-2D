

using Core.Game.Models.Enum;
using LiteNetLib.Utils;

namespace Infrastructure.Network.Packet.Server
{
    public class SVitalsNotification
    {
        public int Index { get; set; }
        public VitalType VitalType { get; set; }
        public double Value { get; set; }
        
        public static SVitalsNotification Create(int index, VitalType vitalType, double value)
        {
            return new SVitalsNotification
            {
                Index = index,
                VitalType = vitalType,
                Value = value
            };
        }
    }
}
