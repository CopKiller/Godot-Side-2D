using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Infrastructure.Network.CustomDataSerializable;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace Infrastructure.Network.Packet.Server
{
    public class SUpdateBody
    {
        public int Index { get; set; }
        public EntityType Type { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; }
        
        public static SUpdateBody Create(int index, EntityType type, Vector2 position, Vector2 velocity, float rotation)
        {
            return new SUpdateBody
            {
                Index = index,
                Type = type,
                Position = position,
                Velocity = velocity,
                Rotation = rotation
            };
        }
    }

}
