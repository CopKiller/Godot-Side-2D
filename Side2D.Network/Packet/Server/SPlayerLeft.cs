

using LiteNetLib.Utils;

namespace Side2D.Network.Packet.Server
{
    public class SPlayerLeft
    {
        public int Index { get; set; }
        
        public static SPlayerLeft Create(int index)
        {
            return new SPlayerLeft
            {
                Index = index
            };
        }
    }
}
