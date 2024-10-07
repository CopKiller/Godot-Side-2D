
using System.Linq.Expressions;
using System.Numerics;
using LiteNetLib;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Player;
using Side2D.Models.Vectors;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Server;
using Side2D.Server.Database.Repositorys;
using Side2D.Server.TempData.Temp.Interface;

namespace Side2D.Server.Network
{
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
            
            _serverPacketProcessor?.ServerLeft(Peer, left, false);
            
            // Clear the player data model
            PlayerDataModel.Clear();
            // Clear the player move model
            PlayerMoveModel.Clear();
            
            TempPlayer.ChangeState(ClientState.Character);
        }

        public void SavePlayerData()
        {
            
            if (TempPlayer.ClientState != ClientState.Game) return;
            
            // Puxa o player dos ultimos dados temporários
            var player = TempPlayer.GetCharacter(TempPlayer.SlotNumber);

            if (player == null) return;
            
            player.Attributes.SetValues(PlayerDataModel.Attributes);
            player.Vitals.SetValues(PlayerDataModel.Vitals);
            player.Position.SetPosition(PlayerMoveModel.Position);
            player.Direction = PlayerMoveModel.Direction;
                
            UpdatePlayerInDatabase?.Invoke(player);
        }

        public Vitals? UpdatePlayerVitals()
        {
            if (TempPlayer.ClientState != ClientState.Game) return null;
            
            // Puxa o player dos ultimos dados temporários
            var player = TempPlayer.GetCharacter(TempPlayer.SlotNumber);

            if (player == null) return null;
            
            player.Vitals.SetValues(PlayerDataModel.Vitals);
            
            return player.Vitals;
        }

    }
}
