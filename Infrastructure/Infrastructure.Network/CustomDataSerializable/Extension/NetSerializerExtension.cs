
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using LiteNetLib.Utils;

namespace Infrastructure.Network.CustomDataSerializable.Extension
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
            var vector2 = new Vector2C
            {
                X = reader.GetFloat(),
                Y = reader.GetFloat()
            };
            return vector2;
        }
        
        public static void Put(this NetDataWriter writer, Vitals val)
        {
            writer.Put(val.Health);
            writer.Put(val.Mana);
            writer.Put(val.MaxHealth);
            writer.Put(val.MaxMana);
        }
        
        public static Vitals GetVitals(this NetDataReader reader)
        {
            var vitals = new Vitals()
            {
                Health = reader.GetDouble(),
                Mana = reader.GetDouble(),
                MaxHealth = reader.GetDouble(),
                MaxMana = reader.GetDouble()
            };
            return vitals;
        }
        
        public static void Put(this NetDataWriter writer, Attributes val)
        {
            writer.Put(val.Strength);
            writer.Put(val.Defense);
            writer.Put(val.Agility);
            writer.Put(val.Intelligence);
            writer.Put(val.Willpower);
        }
        
        public static Attributes GetAttributes(this NetDataReader reader)
        {
            var attributes = new Attributes()
            {
                Strength = reader.GetInt(),
                Defense = reader.GetInt(),
                Agility = reader.GetInt(),
                Intelligence = reader.GetInt(),
                Willpower = reader.GetInt()
            };
            return attributes;
        }
    }
}