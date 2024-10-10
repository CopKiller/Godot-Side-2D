using Infrastructure.Network.CustomDataSerializable;
using LiteNetLib.Utils;

namespace Infrastructure.Network.Packet.Server
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
