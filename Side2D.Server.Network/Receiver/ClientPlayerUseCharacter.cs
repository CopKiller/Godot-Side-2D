using LiteNetLib;
using Side2D.Logger;
using Side2D.Models.Enum;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public async void ClientPlayerUseCharacter(CPlayerUseCharacter obj, NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;

            if (!ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player))
                return;

            if (player.TempPlayer.ClientState != ClientState.Character) return;

            if (player.TempPlayer.CountCharacters() == 0)
            {
                ServerAlert(netPeer, "You don't have any characters!");
                return;
            }

            // Buscar o PlayerDataModel diretamente
            var playerModel = player.TempPlayer.GetCharacter(obj.SlotNumber);
            
            if (playerModel == null)
            {
                ServerAlert(netPeer, "Character not found!");
                return;
            }
            
            // Cria os modelos de dados do jogador
            player.PlayerDataModel = new PlayerDataModel(netPeer.Id, playerModel);
            player.PlayerMoveModel = new PlayerMoveModel(netPeer.Id, playerModel);

            // Atualiza o estado do jogador
            player.TempPlayer.ChangeState(ClientState.Game, obj.SlotNumber);
            
            var changeClientState = new SClientState
            {
                ClientState = player.TempPlayer.ClientState
            };
            SendDataTo(netPeer, changeClientState, DeliveryMethod.ReliableOrdered);

            // Cria o pacote com os dados do jogador
            var packet = new SPlayerData();
            packet.PlayersDataModels.Add(player.PlayerDataModel);
            packet.PlayersMoveModels.Add(player.PlayerMoveModel);

            // Envia os dados do jogador para todos os outros jogadores
            SendDataToAllBut(netPeer, packet, ClientState.Game, DeliveryMethod.ReliableOrdered);

            var otherPlayers = ServerNetworkService.Players.Values
                .Where(x => x.TempPlayer.ClientState == ClientState.Game && x.Index != player.Index);

            var serverClients = otherPlayers as ServerClient[] ?? otherPlayers.ToArray();
            packet.PlayersDataModels.AddRange(serverClients.Select(x => x.PlayerDataModel));
            packet.PlayersMoveModels.AddRange(serverClients.Select(x => x.PlayerMoveModel));

            SendDataTo(netPeer, packet, DeliveryMethod.ReliableOrdered);

            player.UpdatePlayerInDatabase = ServerNetworkService.DatabaseService.PlayerRepository.UpdatePlayerAsync;
        }
    }
}