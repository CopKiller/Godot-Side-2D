using LiteNetLib.Utils;
using Side2D.Network.CustomDataSerializable;

namespace Side2D.Network.Packet.Server
{
    public class SPlayerMove
    {
        public PlayerMoveModel PlayerMoveModel { get; set; }
        
        public static SPlayerMove Create(PlayerMoveModel playerMoveModel)
        {
            return new SPlayerMove
            {
                PlayerMoveModel = playerMoveModel
            };
        }
    }

}
