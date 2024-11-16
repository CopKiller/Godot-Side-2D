
using Core.Game.Interfaces.Attribute.Player;
using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.TempData.Player;
using Core.Game.Models.Enum;
using LiteNetLib;
using Infrastructure.Network.CustomDataSerializable;
using Infrastructure.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public class ServerClient
    {
        public int Index { get; }
        public readonly ITempPlayer TempPlayer;
        public IPhysicPlayer PhysicPlayer { get; private set; }
        public IAttributePlayer AttributePlayer { get; private set; }
        
        public NetPeer Peer { get; }
        public PlayerDataModel PlayerDataModel { get; set; }
        
        private readonly ServerPacketProcessor? _serverPacketProcessor;

        public ServerClient(NetPeer netPeer,
                            ITempPlayer player, 
                            ServerPacketProcessor? serverPacketProcessor)
        {
            Peer = netPeer;
            
            TempPlayer = player;
            
            _serverPacketProcessor = serverPacketProcessor;

            Index = netPeer.Id;
            
            TempPlayer.ChangeState(ClientState.Menu);
        }
        
        public void AddPlayerServices(IPhysicPlayer physicPlayer, IAttributePlayer attributePlayer)
        {
            PhysicPlayer = physicPlayer;
            AttributePlayer = attributePlayer;
        }

        public void Disconnect()
        {
            var left = new SPlayerLeft
            {
                Index = Index
            };
            
            TempPlayer.ChangeState(ClientState.None);
            
            _serverPacketProcessor?.ServerLeft(Peer, left);
        }
        
        public void PlayerSwitchCharacter(int index)
        {
            
            var left = SPlayerLeft.Create(index);
            
            TempPlayer.ChangeState(ClientState.Character);
            
            _serverPacketProcessor?.ServerLeft(Peer, left, false);
            
            // Clear the player data model
            PlayerDataModel.Clear();
        }

    }
}
