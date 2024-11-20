using Core.Game.Models.Enum;
using Core.Game.Models.Validation;
using Infrastructure.Network.CustomDataSerializable;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.NetworkService;

public partial class ServerPacketProcessor
{
    public void ServerSendCharacters(NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

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