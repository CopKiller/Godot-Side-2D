
using LiteNetLib;
using Side2D.Logger;
using Side2D.Models.Enum;
using Side2D.Models.Validation;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ServerSendCharacters(NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;
            
            if (player.TempPlayer.ClientState != ClientState.Character) return;
            
            var myPlayerDataModel = new List<PlayerDataModel>();
            myPlayerDataModel.AddRange(player.TempPlayer.GetCharacters().Select(a => new PlayerDataModel(netPeer.Id, a)));
            
            var slotsCount = myPlayerDataModel.Count;
            
            if (slotsCount < EntityValidator.MaxCharacters)
            {
                var slots = EntityValidator.MaxCharacters - slotsCount;
                
                var usedSlots = myPlayerDataModel.Select(p => p.SlotNumber).ToHashSet();
                var nextSlotNumber = 1;

                for (var i = 0; i < slots; i++)
                {
                    while (usedSlots.Contains(nextSlotNumber))
                    {
                        nextSlotNumber++;
                    }
                    
                    var emptyPlayer = new PlayerDataModel { SlotNumber = nextSlotNumber };
                    myPlayerDataModel.Add(emptyPlayer);
                    
                    usedSlots.Add(nextSlotNumber);
                }
            }
            
            myPlayerDataModel = myPlayerDataModel.OrderBy(p => p.SlotNumber).ToList();
            
            var packet = new SCharacter
            {
                PlayerDataModel = myPlayerDataModel
            };
            
            SendDataTo(netPeer, packet, DeliveryMethod.ReliableOrdered);
        }
    }
}
