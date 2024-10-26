using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Validation;
using Infrastructure.Network.Packet.Client;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public async void ClientCreateCharacter(CCreateCharacter obj, NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

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

        var newPlayer = new PlayerModel(obj.SlotNumber, obj.Name, obj.Vocation, obj.Gender);

        var res = newPlayer.Validate();

        if (res != null)
        {
            ServerAlert(netPeer, res.Message);
            return;
        }

        var result =
            await playerRepository.AddPlayerAsync(player.TempPlayer.AccountId,
                newPlayer);

        if (result != null)
        {
            ServerAlert(netPeer, result.Message);
            return;
        }

        ServerAlert(netPeer, $"Character {newPlayer.Name} created successfully!");

        player.TempPlayer.AddPlayerData(newPlayer);

        ServerSendCharacters(netPeer);
    }
}