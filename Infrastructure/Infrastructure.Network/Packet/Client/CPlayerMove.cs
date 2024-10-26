using Core.Game.Models.Player;
using Infrastructure.Network.CustomDataSerializable;

namespace Infrastructure.Network.Packet.Client
{
    public class CPlayerMove
    {
        public Position Position { get; set; }
    }

}
