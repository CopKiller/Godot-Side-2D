using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using LiteNetLib.Utils;
using Infrastructure.Network.CustomDataSerializable.Extension;

namespace Infrastructure.Network.CustomDataSerializable;

public struct PlayerDataModel : INetSerializable
{
    public int Index { get; set; }
    
    public int SlotNumber { get; set; }
    
    public string Name { get; set; }
    
    public int Level { get; set; }
    
    public Vocation Vocation { get; set; }
    
    public Gender Gender { get; set; }

    public Vitals Vitals { get; set; } = new Vitals();
    
    public IStats IStats { get; set; } = new IStats();
    
    public Position Position { get; set; } = new Position();
    
    public float JumpVelocity { get; set; }
    
    public float Speed { get; set; }
    
    public PlayerDataModel() { }

    public void Clear()
    {
        Index = 0;
        SlotNumber = 0;
        Name = string.Empty;
        Level = 0;
        Vocation = Vocation.None;
        Gender = Gender.Undefined;
        Vitals = null;
        IStats = null;
        Position = null;
        JumpVelocity = 0;
        Speed = 0;
    }

    public PlayerDataModel(int index, PlayerModel playerModel)
    {
        Index = index;
        SlotNumber = playerModel.SlotNumber;
        Name = playerModel.Name;
        Level = playerModel.Level;
        Vocation = playerModel.Vocation;
        Gender = playerModel.Gender;
        
        Vitals = playerModel.Vitals;
        IStats = playerModel.IStats;
        Position = playerModel.Position;
        Position.Index = index;
        
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
        IStats = playerModel.IStats;
        Position = playerModel.Position;
        
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
        writer.Put(IStats);
        writer.Put(Position);
        
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
        IStats = reader.GetAttributes();
        Position = reader.GetPosition();
        
        JumpVelocity = reader.GetFloat();
        Speed = reader.GetFloat();
    }
}