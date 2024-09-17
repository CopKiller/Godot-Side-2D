
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

            var accountModel = new AccountModel()
            {
                Username = obj.AccountRegisterModel.Username,
                Password = obj.AccountRegisterModel.Password,
                Email = obj.AccountRegisterModel.Email
            };
            
            var results = accountModel.Validate();
            
            if (results != null)
            {
                Log.PrintError(results.ToString());
            }
            
            var result = await ServerNetworkService.AccountRepository.AddAccountAsync(accountModel);

            if (result != null)
            {
                Log.PrintError(result.ToString());
            }
            
        }
    }
}
