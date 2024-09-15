using LiteNetLib.Utils;
using Side2D.Models;
using Side2D.Models.Enum;

namespace Side2D.Network.CustomDataSerializable;

public struct AccountRegisterModel : INetSerializable
{
    public int Index { get; set; }
    
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Index);
        writer.Put(Username);
        writer.Put(Password);
        writer.Put(Email);
    }

    public void Deserialize(NetDataReader reader)
    {
        Index = reader.GetInt();
        Username = reader.GetString();
        Password = reader.GetString();
        Email = reader.GetString();
    }
}