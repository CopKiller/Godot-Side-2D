using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ServerUpdateVitals(int index, Vitals vitals)
    {
        players.TryGetValue(index, out var player);
            
        var packet = SPlayerUpdateVitals.Create(index, vitals);
            
        SendDataToAll(packet, ClientState.Game, DeliveryMethod.ReliableUnordered);
    }
}