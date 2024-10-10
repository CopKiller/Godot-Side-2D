
using Core.Game.Models.Player;

namespace Infrastructure.Network.Packet.Server
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
