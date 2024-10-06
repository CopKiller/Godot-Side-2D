using Side2D.Models.Enum;

namespace Side2D.Network.Packet.Server;

public class SClientState
{
    public ClientState ClientState { get; set; }
    
    public static SClientState Create(ClientState clientState)
    {
        return new SClientState
        {
            ClientState = clientState
        };
    }
}