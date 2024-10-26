using Infrastructure.Network.CustomDataSerializable;
using LiteNetLib.Utils;

namespace Infrastructure.Network.Packet.Server
{
    public class SPlayerData
    {
        public List<PlayerDataModel> PlayersDataModels { get; set; } = [];  
    }
}
