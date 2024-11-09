using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ServerVitalsNotification(int index, VitalType vitalType, double value)
    {
        players.TryGetValue(index, out var player);
        
        if (player == null) return;
        
        if (player.TempPlayer.ClientState != ClientState.Game) return;
        
        var packet = SVitalsNotification.Create(index, vitalType, value);
        
        SendDataToAll(packet, ClientState.Game, DeliveryMethod.ReliableUnordered);
    }
}