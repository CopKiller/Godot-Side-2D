
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
        // Deixando escrito aqui, para não esquecer de implementar
        // Preciso remover tudo que não é necessário para o network e deixar apenas o necessário
        // jogando a maioria das coisas aqui pro novo projeto de dados temporários
        // e deixando apenas o necessário para o network aqui, como netpeer, index, accountid, etc...
        // no projeto dos dados temporários terá uma lista de jogadores atrelados ao index, como no network
        // e a cada jogador terá um objeto temporário, que será atualizado a cada tick
        
        public int Index { get; private set; }
        public NetPeer Peer { get; private set; }
        public int AccountId { get; set; }
        public ClientState ClientState { get; set; }
        public List<PlayerModel> PlayerModels { get; private set; } = [];
        public PlayerMoveModel PlayerMoveModel { get; set; } 
        public PlayerDataModel PlayerDataModel { get; set; }
        
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
            
            if (Peer.ConnectionState == ConnectionState.Connected)
                Peer.Disconnect();
            
            _serverPacketProcessor?.ServerLeft(Peer, left);
        }
        
        public void PlayerSwitchCharacter(int index)
        {
            ClientState = ClientState.Character;
            
            var left = new SPlayerLeft
            {
                Index = index
            };
            
            _serverPacketProcessor?.ServerLeft(Peer, left, false);
        }
        
    }
}
