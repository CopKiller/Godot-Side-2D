using LiteNetLib;
using Side2D.Logger;
using Side2D.Models.Enum;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ClientPlayerAttack(CPlayerAttack obj, NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;

            var packet = new SPlayerAttack
            {
                Index = player.Index,
                AttackType = AttackType.Basic
            };
            
            SendDataToAllBut(netPeer, packet, ClientState.Game, DeliveryMethod.ReliableSequenced);
        }
    }
}