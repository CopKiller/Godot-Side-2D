using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.NetworkService;

public partial class ServerPacketProcessor
{
    public void ServerUpdateAttack(int index, EntityType type, bool includeSelf, AttackType attackType)
    {
        
        if (type != EntityType.Player) return;
        
        players.TryGetValue(index, out var player);
        
        if (player == null) return;
        
        if (player.TempPlayer.ClientState != ClientState.Game) return;
        
        var packet = SPlayerAttack.Create(index, attackType);
        
        if (includeSelf)
            SendDataToAll(packet, ClientState.Game, DeliveryMethod.ReliableUnordered);
        else
            SendDataToAllBut(player.Peer, packet, ClientState.Game, DeliveryMethod.ReliableUnordered);
    }
}