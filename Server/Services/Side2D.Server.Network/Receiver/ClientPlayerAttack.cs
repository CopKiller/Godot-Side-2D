using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Client;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ClientPlayerAttack(CPlayerAttack obj, NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player == null) return;
            
        if (player.TempPlayer.ClientState != ClientState.Game) return;
            
        if (player.TempPlayer.Attack == null) return;
            
        if (player.TempPlayer.Attack.CanAttack() == false)
        {
            ServerAlert(netPeer, "Invalid attack!");
            return;
        }

        var packet = SPlayerAttack.Create(player.Index, AttackType.Basic);
            
        SendDataToAllBut(netPeer, packet, ClientState.Game, DeliveryMethod.ReliableSequenced);
    }
}