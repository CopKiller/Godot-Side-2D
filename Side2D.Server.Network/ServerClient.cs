
using System.Linq.Expressions;
using System.Numerics;
using LiteNetLib;
using Side2D.Models;
using Side2D.Models.Enum;
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
        public PlayerMoveModel PlayerMoveModel { get; set; }  = new();
        public PlayerDataModel PlayerDataModel { get; set; } = new();
        public UpdatePlayerDelegate? UpdatePlayerInDatabase { get; set; } = null;
        
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
            var left = new SPlayerLeft
            {
                Index = index
            };
            
            SavePlayerData();
            
            // Notifica jogadores ingame sobre a saida deste player e faz a limpeza no client deles.
            _serverPacketProcessor?.ServerLeft(Peer, left, false);
            
            TempPlayer.ChangeState(ClientState.Character);
        }

        public void SavePlayerData()
        {
            // Lógica para salvar jogador no banco de dados
            
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

    }
}
