using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Client;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public async void ClientLogin(CPlayerLogin obj, NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player == null) return;
            
        if (player.TempPlayer.ClientState != ClientState.Menu) return;
            
        var account = await accountRepository.GetAccountAsync(obj.Username, obj.Password);
            
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
            
        if (players.Values.Any(p => p.TempPlayer.AccountId == account.Value.Id))
        {
            ServerAlert(netPeer, "Account already logged in!");
            return;
        }
            
        ServerAlert(netPeer, $"Account logged in successfully! User: {account.Value.Username}");
        player.TempPlayer.AddAccountData(account.Value);
        player.TempPlayer.ChangeState(ClientState.Character);
            
        ServerSendCharacters(netPeer);
    }
}