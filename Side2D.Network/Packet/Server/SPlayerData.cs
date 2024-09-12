using LiteNetLib.Utils;
using Side2D.Network.CustomDataSerializable;

namespace Side2D.Network.Packet.Server
{
    public class SPlayerData
    {
        public List<PlayerDataModel> PlayersDataModels { get; set; } = [];
        
        public List<PlayerMoveModel> PlayersMoveModels { get; set; } = [];
    }
}
