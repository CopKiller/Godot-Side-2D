

using Core.Game.Models.Vectors;
using LiteNetLib.Utils;

namespace Infrastructure.Network.Packet.Server
{
    public class SPlayerImpact
    {
        public int Index { get; set; }
        public Vector2C ImpactVelocity { get; set; }
        
        public static SPlayerImpact Create(int index, Vector2C impactVelocity)
        {
            return new SPlayerImpact
            {
                Index = index,
                ImpactVelocity = impactVelocity
            };
        }
    }
}
