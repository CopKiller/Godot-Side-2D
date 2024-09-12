using LiteNetLib;
using Side2D.Logger;
using Side2D.Network;
using Side2D.Network.Packet.Client;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor : PacketProcessor
    {
        public readonly Dictionary<int, ServerClient> _players;

        public ServerPacketProcessor(Dictionary<int, ServerClient> players)
        {
            _players = players;
        }

        public void Initialize()
        {
            this.RegisterCustomTypes();

            SubscribePacket();
        }

        public override void SubscribePacket()
        {
            // Register to receive packets  
            this.Subscribe<CPlayerLogin>(ClientLogin);
            this.Subscribe<CPlayerMove>(ClientPlayerMove);
        }

        public void SendDataToAllBut<T>(NetPeer excludePeer, T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
        {
            var excludePeerId = excludePeer.Id;

            var allPlayers = _players.Select(allPlayers => allPlayers.Value)
                .Where(player => player.Peer.Id != excludePeerId)
                .ToList();

            foreach (var player in allPlayers)
            {
                SendDataTo(player.Peer, packet, deliveryMethod);
            }
        }

        public void SendDataToAll<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
        {
            foreach (var player in _players)
            {
                SendDataTo(player.Value.Peer, packet, deliveryMethod);
            }
        }

        public override void SendDataTo<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class
        {
            Log.PrintInfo(packet.GetType().ToString());
            base.SendDataTo(peer, packet, deliveryMethod);
        }
    }
}
