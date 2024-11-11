using Core.Game.Models.Player;
using Infrastructure.Network.CustomDataSerializable;
using Microsoft.Xna.Framework;

namespace Infrastructure.Network.Packet.Client
{
    public class CPlayerMove
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
    }

}
