using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;

namespace Side2D.Server.Physics;

public class NetworkPhysic : INetworkPhysic
{
    public event ServerUpdatePosition? OnServerUpdatePosition;
    public event ServerUpdateKnockback? OnServerUpdateKnockback;
    public event ServerUpdateAttack? OnServerUpdateAttack;

    public void ServerUpdatePosition(int index, EntityType type, bool includeSelf)
    {
        OnServerUpdatePosition?.Invoke(index, type, includeSelf);
    }

    public void ServerUpdateKnockback(int index, EntityType type, bool includeSelf, VectorTwo newPosition)
    {
        OnServerUpdateKnockback?.Invoke(index, type, includeSelf, newPosition);
    }

    public void ServerUpdateAttack(int index, EntityType type, bool includeSelf, AttackType attackType)
    {
        OnServerUpdateAttack?.Invoke(index, type, includeSelf, attackType);
    }
}