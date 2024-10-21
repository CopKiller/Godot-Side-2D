using Core.Game.Models.Enum;

namespace Infrastructure.Network.Packet.Server;

public class SClientState
{
    public int Index { get; set; }
    public ClientState ClientState { get; set; }
    
    public static SClientState Create(int index, ClientState clientState)
    {
        return new SClientState
        {
            Index = index,
            ClientState = clientState
        };
    }
}