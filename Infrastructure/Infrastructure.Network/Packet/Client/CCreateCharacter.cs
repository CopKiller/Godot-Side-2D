
using Core.Game.Models.Enum;

namespace Infrastructure.Network.Packet.Client
{
    public class CCreateCharacter
    {
        public int SlotNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public Vocation Vocation { get; set; }
    }
}
