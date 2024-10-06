
using LiteNetLib;
using Side2D.Logger;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Player;
using Side2D.Models.Validation;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;
using Side2D.Server.TempData.Temp;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public async void ClientCreateCharacter(CCreateCharacter obj, NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;
            
            if (player.TempPlayer.ClientState != ClientState.Character) return;
            
            if (player.TempPlayer.CountCharacters() >= EntityValidator.MaxCharacters)
            {
                ServerAlert(netPeer, $"You can only have {EntityValidator.MaxCharacters} characters!");
                return;
            }
            
            if (player.TempPlayer.ExistsCharacter(obj.SlotNumber))
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

            var result = await ServerNetworkService.DatabaseService.PlayerRepository.AddPlayerAsync(player.TempPlayer.AccountId, newPlayer);
            
            if (result != null)
            {
                ServerAlert(netPeer, result.Message);
                return;
            }
            
            ServerAlert(netPeer, $"Character {newPlayer.Name} created successfully!");
            
            player.TempPlayer.UpdatePlayerData(newPlayer);

            ServerSendCharacters(netPeer);
        }
    }
}
