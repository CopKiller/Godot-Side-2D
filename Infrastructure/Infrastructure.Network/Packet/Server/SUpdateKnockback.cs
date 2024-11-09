

using Core.Game.Models.Vectors;
using LiteNetLib.Utils;

namespace Infrastructure.Network.Packet.Server
{
    public class SUpdateKnockback
    {
        public int Index { get; set; }
        public VectorTwo Position { get; set; }
        
        public static SUpdateKnockback Create(int index, VectorTwo position)
        {
            return new SUpdateKnockback
            {
                Index = index,
                Position = position
            };
        }
    }
}
