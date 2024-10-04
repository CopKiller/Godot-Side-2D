
using LiteNetLib;
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
            
            player.PlayerModels.AddRange(account.Value.Players);
            
            ServerSendCharacters(netPeer);
        }
    }
}
