

using Core.Game.Models.Vectors;
using LiteNetLib.Utils;

namespace Infrastructure.Network.Packet.Server
{
    public class SPlayerImpact
    {
        public int Index { get; set; }
        public VectorTwo ImpactVelocity { get; set; }
        
        public static SPlayerImpact Create(int index, VectorTwo impactVelocity)
        {
            return new SPlayerImpact
            {
                Index = index,
                ImpactVelocity = impactVelocity
            };
        }
    }
}
