

using Core.Game.Models.Enum;

namespace Infrastructure.Network.Packet.Server
{
    public class SPlayerAttack
    {
        public int Index { get; set; }
        public AttackType AttackType { get; set; }
        
        public static SPlayerAttack Create(int index, AttackType attackType)
        {
            return new SPlayerAttack
            {
                Index = index,
                AttackType = attackType
            };
        }
    }

}
