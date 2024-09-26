
using LiteNetLib;
using Side2D.Cryptography;
using Side2D.Logger;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Validation;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public async void ClientLogin(CPlayerLogin obj, NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;
            
            if (player.ClientState != ClientState.Menu) return;
            
            var account = await ServerNetworkService.AccountRepository.GetAccountAsync(obj.Username, obj.Password);
            
            if (account.Error != null)
            {
                ServerAlert(netPeer, account.Error.Message);
                return;
            }
            
            if (account.Value == null)
            {
                ServerAlert(netPeer, "Account not found!");
                return;
            }
            
            if (ServerNetworkService.Players.Values.Any(p => p.AccountId == account.Value.Id))
            {
                ServerAlert(netPeer, "Account already logged in!");
                return;
            }
            
            ServerAlert(netPeer, $"Account logged in successfully! User: {account.Value.Username}");
            
            player.ClientState = ClientState.Character;
            player.AccountId = account.Value.Id;
            
            var changeClientState = new SClientState()
            {
                ClientState = player.ClientState
            };
            
            SendDataTo(netPeer, changeClientState, DeliveryMethod.ReliableOrdered);
            
            // Adiciona os PlayerModels da conta ao Player
            player.PlayerModels.AddRange(account.Value.Players);

            // Cria uma nova lista para os PlayerModels
            var myPlayerDataModel = new List<PlayerDataModel>();
            myPlayerDataModel.AddRange(player.PlayerModels.Select(a => new PlayerDataModel(netPeer.Id, a)));

            // Ordena os jogadores existentes por SlotNumber
            myPlayerDataModel = myPlayerDataModel.OrderBy(p => p.SlotNumber).ToList();

            // Conta quantos jogadores estão na lista
            var slotsCount = myPlayerDataModel.Count;

            // Preenche os slots vazios, se necessário
            if (slotsCount < EntityValidator.MaxCharacters)
            {
                var slots = EntityValidator.MaxCharacters - slotsCount;

                // Encontrar o próximo SlotNumber disponível
                var usedSlots = myPlayerDataModel.Select(p => p.SlotNumber).ToHashSet();
                var nextSlotNumber = 1;

                for (var i = 0; i < slots; i++)
                {
                    // Encontra o próximo SlotNumber disponível que não esteja em uso
                    while (usedSlots.Contains(nextSlotNumber))
                    {
                        nextSlotNumber++;
                    }

                    // Adiciona um PlayerDataModel vazio com o próximo SlotNumber disponível
                    var emptyPlayer = new PlayerDataModel { SlotNumber = nextSlotNumber };
                    myPlayerDataModel.Add(emptyPlayer);
        
                    // Marca o SlotNumber como usado
                    usedSlots.Add(nextSlotNumber);
                }
            }

            // Ordena a lista final pela ordem dos SlotNumbers
            myPlayerDataModel = myPlayerDataModel.OrderBy(p => p.SlotNumber).ToList();

            // Cria o pacote com a lista de PlayerDataModel
            var packet = new SCharacter
            {
                PlayerDataModel = myPlayerDataModel
            };

            Log.Print($"SCharacter slots: {myPlayerDataModel.Count}");
            
            SendDataTo(netPeer, packet, DeliveryMethod.ReliableOrdered);


            /*var packet = new SPlayerData();

            packet.PlayersDataModels.Add(ServerNetworkService.Players.Values.FirstOrDefault(x => x.PlayerDataModel.Index == netPeer.Id)!.PlayerDataModel);
            packet.PlayersMoveModels.Add(ServerNetworkService.Players.Values.FirstOrDefault(x => x.PlayerMoveModel.Index == netPeer.Id)!.PlayerMoveModel);

            SendDataToAll(packet, DeliveryMethod.ReliableOrdered);

            packet.PlayersDataModels.Clear();
            packet.PlayersMoveModels.Clear();

            packet.PlayersDataModels.AddRange(ServerNetworkService.Players.Values.Select(p => p.PlayerDataModel).Where(x => x.Index != netPeer.Id));
            packet.PlayersMoveModels.AddRange(ServerNetworkService.Players.Values.Select(p => p.PlayerMoveModel).Where(x => x.Index != netPeer.Id));

            SendDataTo(netPeer, packet, DeliveryMethod.ReliableOrdered);*/
        }
    }
}
