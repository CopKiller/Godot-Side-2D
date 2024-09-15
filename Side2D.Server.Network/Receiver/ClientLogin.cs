
using LiteNetLib;
using Side2D.Models.Enum;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ClientLogin(CPlayerLogin obj, NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;
            
            if (player.ClientState != ClientState.Menu) return;
            
            player.ClientState = ClientState.Game;
            var changeClientState = new SClientState()
            {
                ClientState = player.ClientState
            };
            
            SendDataTo(netPeer, changeClientState, DeliveryMethod.ReliableOrdered);
            

            var packet = new SPlayerData();
            
            packet.PlayersDataModels.Add(ServerNetworkService.Players.Values.FirstOrDefault(x => x.PlayerDataModel.Index == netPeer.Id)!.PlayerDataModel);
            packet.PlayersMoveModels.Add(ServerNetworkService.Players.Values.FirstOrDefault(x => x.PlayerMoveModel.Index == netPeer.Id)!.PlayerMoveModel);
            
            SendDataToAll(packet, DeliveryMethod.ReliableOrdered);
            
            packet.PlayersDataModels.Clear();
            packet.PlayersMoveModels.Clear();
            
            packet.PlayersDataModels.AddRange(ServerNetworkService.Players.Values.Select(p => p.PlayerDataModel).Where(x => x.Index != netPeer.Id));
            packet.PlayersMoveModels.AddRange(ServerNetworkService.Players.Values.Select(p => p.PlayerMoveModel).Where(x => x.Index != netPeer.Id));
            
            SendDataTo(netPeer, packet, DeliveryMethod.ReliableOrdered);
        }
    }
}
