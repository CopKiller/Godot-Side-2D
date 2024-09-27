using LiteNetLib.Utils;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Player;
using Side2D.Models.Vectors;
using Side2D.Network.CustomDataSerializable.Extension;

namespace Side2D.Network.CustomDataSerializable;

public struct PlayerDataModel : INetSerializable
{
    public int Index { get; set; }
    
    public int SlotNumber { get; set; }
    
    public string Name { get; set; }
    
    public int Level { get; set; }
    
    public Vocation Vocation { get; set; }
    
    public Gender Gender { get; set; }

    public Vitals Vitals { get; set; } = new Vitals();
    
    public Attributes Attributes { get; set; } = new Attributes();
    
    public float JumpVelocity { get; set; }
    
    public float Speed { get; set; }
    
    public PlayerDataModel() { }
    public PlayerDataModel(int index, PlayerModel playerModel)
    {
        Index = index;
        SlotNumber = playerModel.SlotNumber;
        Name = playerModel.Name;
        Level = playerModel.Level;
        Vocation = playerModel.Vocation;
        Gender = playerModel.Gender;
        
        Vitals = playerModel.Vitals;
        Attributes = playerModel.Attributes;
        
        JumpVelocity = playerModel.JumpVelocity;
        Speed = playerModel.Speed;
    }

    public void SetValues(PlayerDataModel playerModel)
    {
        SlotNumber = playerModel.SlotNumber;
        Name = playerModel.Name;
        Level = playerModel.Level;
        Vocation = playerModel.Vocation;
        
        Vitals = playerModel.Vitals;
        Attributes = playerModel.Attributes;
        
        JumpVelocity = playerModel.JumpVelocity;
        Speed = playerModel.Speed;
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Index);
        writer.Put(SlotNumber);
        writer.Put(Name);
        writer.Put(Level);
        writer.Put((byte)Vocation);
        writer.Put((byte)Gender);
        
        writer.Put(Vitals);
        writer.Put(Attributes);
        
        writer.Put(JumpVelocity);
        writer.Put(Speed);
    }

    public void Deserialize(NetDataReader reader)
    {
        Index = reader.GetInt();
        SlotNumber = reader.GetInt();
        Name = reader.GetString();
        Level = reader.GetInt();
        Vocation = (Vocation)reader.GetByte();
        Gender = (Gender)reader.GetByte();
        
        Vitals = reader.GetVitals();
        Attributes = reader.GetAttributes();
        
        JumpVelocity = reader.GetFloat();
        Speed = reader.GetFloat();
    }
}