using Core.Game.Models.Player;
using Infrastructure.Network.CustomDataSerializable;
using LiteNetLib.Utils;

namespace Infrastructure.Network.Packet.Server
{
    public class SPlayerMove
    {
        public Position Position { get; set; }
        
        public static SPlayerMove Create(Position playerMoveModel)
        {
            return new SPlayerMove
            {
                Position = playerMoveModel
            };
        }
    }

}
