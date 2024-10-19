using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using Infrastructure.Network.CustomDataSerializable.Extension;
using LiteNetLib.Utils;

namespace Infrastructure.Network.CustomDataSerializable;

public struct PlayerMoveModel : INetSerializable
{
    public int Index { get; set; }
    public bool IsMoving { get; set; }
    public Vector2C? Velocity { get; set; } = new Vector2C();
    public Position? Position { get; set; } = new Position();

    public PlayerMoveModel()
    {
    }

    public void Clear()
    {
        Index = 0;
        IsMoving = false;
        Velocity = null;
        Position = null;
    }

    public PlayerMoveModel(int index, PlayerModel playerModel)
    {
        Index = index;
        IsMoving = false;
        Velocity = Vector2C.Zero;
        Position = playerModel.Position;
    }

    public void Deserialize(NetDataReader reader)
    {
        Index = reader.GetInt();
        IsMoving = reader.GetBool();
        Velocity = reader.GetVector2();
        Position = reader.GetPosition();
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Index);
        writer.Put(IsMoving);
        writer.Put(Velocity);
        writer.Put(Position);
    }

    public void SetValues(PlayerMoveModel playerMoveModel)
    {
        Index = playerMoveModel.Index;
        IsMoving = playerMoveModel.IsMoving;
        Velocity.SetValues(playerMoveModel.Velocity);
        Position.SetPosition(playerMoveModel.Position);
    }

    public override string ToString()
    {
        return
            $"Index: {Index}, IsMoving: {IsMoving}, Velocity: {Velocity.ToString()}, Direction: {Position.Direction}, Position: {Position.ToString()}";
    }
}