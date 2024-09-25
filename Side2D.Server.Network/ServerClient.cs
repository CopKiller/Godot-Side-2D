
using System.Linq.Expressions;
using System.Numerics;
using LiteNetLib;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Vectors;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public class ServerClient
    {
        public int Index { get; private set; }
        public NetPeer Peer { get; private set; }
        public int AccountId { get; set; }

        public List<PlayerModel> PlayerModels { get; private set; } = [];
        
        public PlayerMoveModel PlayerMoveModel { get; set; } 
        
        public PlayerDataModel PlayerDataModel { get; set; }
        
        public ClientState ClientState { get; set; }

        private readonly ServerPacketProcessor? _serverPacketProcessor;

        public ServerClient(NetPeer netPeer, ServerPacketProcessor? serverPacketProcessor)
        {
            _serverPacketProcessor = serverPacketProcessor;

            Peer = netPeer;

            Index = netPeer.Id;
            
            ClientState = ClientState.Menu;
        }

        public void Disconnect()
        {
            var left = new SPlayerLeft
            {
                Index = Index
            };
            
            Peer.Disconnect();
            _serverPacketProcessor?.ServerLeft(Peer, left);
        }
        
    }
}
