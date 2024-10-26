using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;

public interface INetworkPhysic
{
    event ServerUpdatePosition OnServerUpdatePosition;
    event ServerUpdateKnockback OnServerUpdateKnockback;
    event ServerUpdateAttack OnServerUpdateAttack;
    
    void ServerUpdatePosition(int index, EntityType type, bool includeSelf);
    
    void ServerUpdateKnockback(int index, EntityType type, bool includeSelf, VectorTwo newPosition);
    
    void ServerUpdateAttack(int index, EntityType type, bool includeSelf, AttackType attackType);
}