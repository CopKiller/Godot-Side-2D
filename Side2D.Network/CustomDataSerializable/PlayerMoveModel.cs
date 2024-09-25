
using Side2D.Network.CustomDataSerializable.Extension;
using LiteNetLib.Utils;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Vectors;

namespace Side2D.Network.CustomDataSerializable
{
    public struct PlayerMoveModel : INetSerializable
    {
        public PlayerMoveModel() { }
        
        public PlayerMoveModel(int index, PlayerModel playerModel)
        {
            Index = index;
            IsMoving = false;
            Velocity = Vector2C.Zero;
            Direction = playerModel.Direction;
            Position = playerModel.Position;
        }

        public int Index { get; set; }
        public bool IsMoving { get; set; }
        public Vector2C Velocity { get; set; }
        public Direction Direction { get; set; }
        public Vector2C Position { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Index = reader.GetInt();
            IsMoving = reader.GetBool();
            Velocity = reader.GetVector2();
            Direction = (Direction)reader.GetByte();
            Position = reader.GetVector2();
        }
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Index);
            writer.Put(IsMoving);
            writer.Put(Velocity);
            writer.Put((byte)Direction);
            writer.Put(Position);
        }
        
        public void ConterToMove(int index, PlayerModel playerModel)
        {
            Index = index;
            IsMoving = false;
            Velocity = Vector2C.Zero;
            Direction = playerModel.Direction;
            Position = playerModel.Position;
        }
        
        public override string ToString()
        {
            return $"Index: {Index}, IsMoving: {IsMoving}, Velocity: {Velocity.ToString()}, Direction: {Direction}, Position: {Position.ToString()}";
        }
    }
}
