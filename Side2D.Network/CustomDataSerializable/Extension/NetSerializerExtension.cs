using System.Numerics;
using LiteNetLib.Utils;
using Side2D.Models.Vectors;

namespace Side2D.Network.CustomDataSerializable.Extension
{
    public static class NetSerializerExtension
    {
        public static void Put(this NetDataWriter writer, Vector2C vector)
        {
            writer.Put(vector.X);
            writer.Put(vector.Y);
        }

        public static Vector2C GetVector2(this NetDataReader reader)
        {
            var vector = new Vector2C(reader.GetFloat(), reader.GetFloat());
            return vector;
        }
    }
}