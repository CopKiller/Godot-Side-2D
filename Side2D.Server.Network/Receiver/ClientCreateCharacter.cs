
using LiteNetLib;
using Side2D.Logger;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Player;
using Side2D.Models.Validation;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public async void ClientCreateCharacter(CCreateCharacter obj, NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;
            
            if (player.ClientState != ClientState.Character) return;
            
            if (player.PlayerModels.Count >= EntityValidator.MaxCharacters)
            {
                ServerAlert(netPeer, $"You can only have {EntityValidator.MaxCharacters} characters!");
                return;
            }
            
            if (player.PlayerModels.Exists(p => p.SlotNumber == obj.SlotNumber))
            {
                ServerAlert(netPeer, "Character already exists in slot!");
                return;
            }
            
            if (obj.SlotNumber is < 1 or > EntityValidator.MaxCharacters)
            {
                ServerAlert(netPeer, "Invalid slot number!");
                return;
            }

            var newPlayer = new PlayerModel()
            {
                SlotNumber = obj.SlotNumber,
                Name = obj.Name,
                Attributes = new Attributes(),
                Vitals = new Vitals(),
                Direction = Direction.Right,
                Gender = obj.Gender,
                Vocation = obj.Vocation,
                Position = EntityValidator.DefaultPosition
            };

            var res = newPlayer.Validate();
            
            if (res != null)
            {
                ServerAlert(netPeer, res.Message);
                return;
            }

            var result = await ServerNetworkService.PlayerRepository.AddPlayerAsync(player.AccountId, newPlayer);
            
            if (result != null)
            {
                ServerAlert(netPeer, result.Message);
                return;
            }
            
            ServerAlert(netPeer, $"Character {newPlayer.Name} created successfully!");
            
            player.PlayerModels.Add(newPlayer);

            // Cria uma nova lista para os PlayerModels
            var myPlayerDataModel = new List<PlayerDataModel>();
            myPlayerDataModel.AddRange(player.PlayerModels.Select(a => new PlayerDataModel(netPeer.Id, a)));

            // Ordena os jogadores existentes por SlotNumber
            //myPlayerDataModel = myPlayerDataModel.OrderBy(p => p.SlotNumber).ToList();

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
        }
    }
}
