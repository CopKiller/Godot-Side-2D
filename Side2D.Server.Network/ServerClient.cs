
using System.Numerics;
using LiteNetLib;
using Side2D.Models.Enum;
using Side2D.Models.Vectors;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public class ServerClient
    {
        
        public int Index { get; set; }
        public NetPeer Peer { get; set; }
        public PlayerDataModel PlayerDataModel { get; set; }
        public PlayerMoveModel PlayerMoveModel { get; set; }
        
        public ClientState ClientState { get; set; }


        private readonly ServerPacketProcessor? _serverPacketProcessor;

        public ServerClient(NetPeer netPeer, ServerPacketProcessor? serverPacketProcessor)
        {
            _serverPacketProcessor = serverPacketProcessor;

            Peer = netPeer;

            Index = netPeer.Id;
            
            PlayerDataModel = new PlayerDataModel
            {
                Index = Index,
                Name = "Player" + Index,
                Vocation = Vocation.Archer,
            };

            PlayerMoveModel = new PlayerMoveModel
            {
                Index = Index,
                Velocity = Vector2C.Zero,
                Direction = Direction.Right,
                Position = new Vector2C(400, 300),
            };
            
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
