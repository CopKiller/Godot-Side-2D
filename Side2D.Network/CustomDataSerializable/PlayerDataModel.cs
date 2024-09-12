using LiteNetLib.Utils;
using Side2D.Models;
using Side2D.Models.Enum;

namespace Side2D.Network.CustomDataSerializable;

public struct PlayerDataModel : INetSerializable
{
    public int Index { get; set; }
    
    public string Name { get; set; }
    
    public Gender Gender { get; set; }
    
    public float JumpVelocity { get; set; }
    
    public float Speed { get; set; }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Index);
        writer.Put(Name);
        writer.Put((byte)Gender);
        writer.Put(JumpVelocity);
        writer.Put(Speed);
    }

    public void Deserialize(NetDataReader reader)
    {
        Index = reader.GetInt();
        Name = reader.GetString();
        Gender = (Gender)reader.GetByte();
        JumpVelocity = reader.GetFloat();
        Speed = reader.GetFloat();
    }

    public void ConvertToData(PlayerModel playerModel)
    {
        Index = playerModel.Id;
        Name = playerModel.Name;
        Gender = playerModel.Gender;
        JumpVelocity = playerModel.JumpVelocity;
        Speed = playerModel.Speed;
    }
}