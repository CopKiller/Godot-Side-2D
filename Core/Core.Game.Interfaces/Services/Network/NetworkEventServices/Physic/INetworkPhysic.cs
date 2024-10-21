using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;

public interface INetworkPhysic
{
    event ServerUpdatePosition OnServerUpdatePosition;
    event ServerPlayerImpact OnServerPlayerImpact;
    event ServerPlayerAttack OnServerPlayerAttack;
    
    void ServerUpdatePosition(int playerIndex, bool includeSelf);
    
    // TODO: Falta implementar o impact aqui ainda...
    void ServerPlayerImpact(int playerIndex, Vector2C impactVelocity);
    
    void ServerPlayerAttack(int playerIndex, AttackType attackType);
}