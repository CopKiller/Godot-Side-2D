using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.NetworkService;

public partial class ServerPacketProcessor
{
    public void ServerUpdateVitals(int index)
    {
        players.TryGetValue(index, out var player);
            
        if (player == null) return;
        
        if (player.TempPlayer.ClientState != ClientState.Game) return;
        
        var packet = SPlayerUpdateVitals.Create(index, player.PlayerDataModel.Vitals);
            
        SendDataTo(player.Peer ,packet, DeliveryMethod.ReliableUnordered);
    }
}