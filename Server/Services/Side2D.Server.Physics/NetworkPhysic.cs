using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;

namespace Side2D.Server.Physics;

public class NetworkPhysic : INetworkPhysic
{
    public event ServerUpdatePosition? OnServerUpdatePosition;
    public event ServerPlayerImpact? OnServerPlayerImpact;
    public event ServerPlayerAttack? OnServerPlayerAttack;

    public void ServerUpdatePosition(int playerIndex, bool includeSelf)
    {
        OnServerUpdatePosition?.Invoke(playerIndex, includeSelf);
    }
    
    public void ServerPlayerImpact(int playerIndex, Vector2C impactVelocity)
    {
        OnServerPlayerImpact?.Invoke(playerIndex, impactVelocity);
    }

    public void ServerPlayerAttack(int playerIndex, AttackType attackType)
    {
        OnServerPlayerAttack?.Invoke(playerIndex, attackType);
    }
}