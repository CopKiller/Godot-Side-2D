
using LiteNetLib;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ClientLogin(CPlayerLogin obj, NetPeer netPeer)
        {
            _players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;

            var packet = new SPlayerData();
            
            packet.PlayersDataModels.Add(_players.Values.FirstOrDefault(x => x.PlayerDataModel.Index == netPeer.Id)!.PlayerDataModel);
            packet.PlayersMoveModels.Add(_players.Values.FirstOrDefault(x => x.PlayerMoveModel.Index == netPeer.Id)!.PlayerMoveModel);
            
            SendDataToAll(packet, DeliveryMethod.ReliableOrdered);
            
            packet.PlayersDataModels.Clear();
            packet.PlayersMoveModels.Clear();
            
            packet.PlayersDataModels.AddRange(_players.Values.Select(p => p.PlayerDataModel).Where(x => x.Index != netPeer.Id));
            packet.PlayersMoveModels.AddRange(_players.Values.Select(p => p.PlayerMoveModel).Where(x => x.Index != netPeer.Id));
            
            SendDataTo(netPeer, packet, DeliveryMethod.ReliableOrdered);
        }
    }
}
