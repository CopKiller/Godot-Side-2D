﻿using LiteNetLib;
using Side2D.Logger;
using Side2D.Network;
using Side2D.Network.Packet.Client;
using Side2D.Server.Network.Interfaces;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor(INetworkService serverNetworkService)
        : PacketProcessor, IServerPacketProcessor
    {
        private ServerNetworkService ServerNetworkService { get; set; } = (ServerNetworkService)serverNetworkService;

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
            this.Subscribe<CAccountRegister>(ClientRegister);
        }

        public void SendDataToAllBut<T>(NetPeer excludePeer, T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
        {
            var excludePeerId = excludePeer.Id;

            if (ServerNetworkService.Players == null) return;
            
            var allPlayers = ServerNetworkService.Players.Select(allPlayers => allPlayers.Value)
                .Where(player => player.Peer.Id != excludePeerId)
                .ToList();

            foreach (var player in allPlayers)
            {
                SendDataTo(player.Peer, packet, deliveryMethod);
            }
        }

        public void SendDataToAll<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
        {
            if (ServerNetworkService.Players == null) return;
            foreach (var player in ServerNetworkService.Players)
            {
                SendDataTo(player.Value.Peer, packet, deliveryMethod);
            }
        }

        public void SendDataTo<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
        {
            Log.PrintInfo(packet.GetType().ToString());
            base.Send(peer, packet, deliveryMethod);
        }
    }
}
