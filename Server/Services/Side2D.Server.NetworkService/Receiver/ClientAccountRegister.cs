using Core.Game.Models;
using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Client;
using LiteNetLib;

namespace Side2D.Server.NetworkService;

public partial class ServerPacketProcessor
{
    public async void ClientAccountRegister(CAccountRegister obj, NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player == null) return;

        if (player.TempPlayer.ClientState != ClientState.Menu) return;

        var account = new AccountModel()
        {
            Username = obj.Username,
            Password = obj.Password,
            Email = obj.Email
        };

        var exception = account.Validate();
        if (exception != null)
        {
            ServerAlert(netPeer, exception.Message);
            return;
        }

        var result = await accountRepository.AddAccountAsync(account);

        if (result != null)
        {
            ServerAlert(netPeer, result.Message);
            return;
        }

        ServerAlert(netPeer, "Account created successfully!");

        var packet = CPlayerLogin.Create(obj.Username, obj.Password);
        ClientLogin(packet, netPeer);
    }
}