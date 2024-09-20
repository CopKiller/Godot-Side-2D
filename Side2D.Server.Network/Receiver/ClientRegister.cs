
using LiteNetLib;
using Side2D.Logger;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public async void ClientRegister(CAccountRegister obj, NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;
            
            if (player.ClientState != ClientState.Menu) return;
            
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

            var result = await ServerNetworkService.AccountRepository.AddAccountAsync(account);
            
            if (result != null)
            {
                ServerAlert(netPeer, result.Message);
                return;
            }
            
            ServerAlert(netPeer, "Account created successfully!");
            
            //player.ClientState = ClientState.Game;
        }
    }
}
