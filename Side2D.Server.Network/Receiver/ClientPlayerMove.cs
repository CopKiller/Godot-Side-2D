using LiteNetLib;
using Side2D.Logger;
using Side2D.Models.Enum;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ClientPlayerMove(CPlayerMove obj, NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;

            var receivedMove = obj.PlayerMoveModel;
            receivedMove.Index = netPeer.Id;
            
            player.PlayerMoveModel = receivedMove;
            
            var packet = new SPlayerMove
            {
                PlayerMoveModel = player.PlayerMoveModel,
            };
            
            SendDataToAllBut(netPeer, packet, ClientState.Game, DeliveryMethod.ReliableSequenced);
        }
    }
}