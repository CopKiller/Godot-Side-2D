
using Side2D.Network.CustomDataSerializable.Extension;
using LiteNetLib.Utils;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Player;
using Side2D.Models.Vectors;

namespace Side2D.Network.CustomDataSerializable
{
    public struct PlayerMoveModel : INetSerializable
    {
        public int Index { get; set; }
        public bool IsMoving { get; set; }
        public Vector2C? Velocity { get; set; } = new Vector2C();
        public Direction Direction { get; set; }
        public Vector2C? Position { get; set; } = new Vector2C();
        
        public PlayerMoveModel() { }
        
        public void Clear()
        {
            Index = 0;
            IsMoving = false;
            Velocity = null;
            Direction = Direction.Right;
            Position = null;
        }
        
        public PlayerMoveModel(int index, PlayerModel playerModel)
        {
            Index = index;
            IsMoving = false;
            Velocity = Vector2C.Zero;
            Direction = playerModel.Direction;
            Position = playerModel.Position;
        }

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
        
        public void SetValues(PlayerMoveModel playerMoveModel)
        {
            Index = playerMoveModel.Index;
            IsMoving = playerMoveModel.IsMoving;
            Velocity.SetValues(playerMoveModel.Velocity.X, playerMoveModel.Velocity.Y);
            Direction = playerMoveModel.Direction;
            Position.SetValues(playerMoveModel.Position.X, playerMoveModel.Position.Y);
        }
        
        public override string ToString()
        {
            return $"Index: {Index}, IsMoving: {IsMoving}, Velocity: {Velocity.ToString()}, Direction: {Direction}, Position: {Position.ToString()}";
        }
    }
}
