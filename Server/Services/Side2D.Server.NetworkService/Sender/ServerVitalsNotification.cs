using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.NetworkService;

public partial class ServerPacketProcessor
{
    public void ServerVitalsNotification(int index, VitalsType vitalsType, double value)
    {
        players.TryGetValue(index, out var player);
        
        if (player == null) return;
        
        if (player.TempPlayer.ClientState != ClientState.Game) return;
        
        var packet = SVitalsNotification.Create(index, vitalsType, value);
        
        SendDataToAll(packet, ClientState.Game, DeliveryMethod.ReliableUnordered);
    }
}