using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ServerPlayerAttack(int index, AttackType attackType)
    {
        players.TryGetValue(index, out var player);
        
        if (player == null) return;
        
        if (player.TempPlayer.ClientState != ClientState.Game) return;
        
        var packet = SPlayerAttack.Create(index, attackType);
        
        SendDataToAllBut(player.Peer, packet, ClientState.Game, DeliveryMethod.ReliableUnordered);
    }
}