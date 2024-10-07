
using LiteNetLib;
using Side2D.Logger;
using Side2D.Models.Enum;
using Side2D.Models.Player;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ServerUpdateVitals(int index)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(index, out var player);

            var vitals = player?.UpdatePlayerVitals();
            
            if (vitals == null) return;
            
            var packet = SPlayerUpdateVitals.Create(index, vitals);
            
            SendDataToAll(packet, ClientState.Game, DeliveryMethod.ReliableUnordered);
        }
    }
}
