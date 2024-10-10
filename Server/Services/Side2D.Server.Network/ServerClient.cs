
using Core.Game.Models;
using Core.Game.Models.Enum;
using LiteNetLib;
using Infrastructure.Network.CustomDataSerializable;
using Infrastructure.Network.Packet.Server;
using Side2D.Server.TempData.Temp.Interface;

namespace Side2D.Server.Network
{
    public delegate Task<bool> UpdatePlayerDelegate(PlayerModel playerModel);
    public class ServerClient
    {
        public int Index { get; }
        public readonly ITempPlayer TempPlayer;
        public NetPeer Peer { get; }
        public PlayerMoveModel PlayerMoveModel { get; set; }
        public PlayerDataModel PlayerDataModel { get; set; }
        public UpdatePlayerDelegate? UpdatePlayerInDatabase { get; set; }
        
        private readonly ServerPacketProcessor? _serverPacketProcessor;

        public ServerClient(NetPeer netPeer, ITempPlayer player, ServerPacketProcessor? serverPacketProcessor)
        {
            Peer = netPeer;
            
            TempPlayer = player;
            
            _serverPacketProcessor = serverPacketProcessor;

            Index = netPeer.Id;
            
            TempPlayer.ChangeState(ClientState.Menu);
        }

        public void Disconnect()
        {
            
            var left = new SPlayerLeft
            {
                Index = Index
            };
            
            SavePlayerData();
            
            _serverPacketProcessor?.ServerLeft(Peer, left);
            
            TempPlayer.ChangeState(ClientState.None);
        }
        
        public void PlayerSwitchCharacter(int index)
        {
            
            var left = SPlayerLeft.Create(index);
            
            SavePlayerData();
            
            TempPlayer.ChangeState(ClientState.Character);
            
            _serverPacketProcessor?.ServerLeft(Peer, left, false);
            
            // Clear the player data model
            PlayerDataModel.Clear();
            // Clear the player move model
            PlayerMoveModel.Clear();
        }

        public void SavePlayerData()
        {
            
            if (TempPlayer.ClientState != ClientState.Game) return;
            
            // Puxa o player model dos ultimos dados temporários, apenas para atualizar dados que não foram por referencia.
            var player = TempPlayer.GetCharacter(TempPlayer.SlotNumber);

            if (player == null) return;
            
            // Atributos comentados ja possuem referencia de classe
            //player.Attributes.SetValues(PlayerDataModel.Attributes);
            //player.Vitals.SetValues(PlayerDataModel.Vitals);
            //player.Position.SetPosition(PlayerMoveModel.Position);
            player.Direction = PlayerMoveModel.Direction;
                
            UpdatePlayerInDatabase?.Invoke(player);
        }

    }
}
