using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;
using Microsoft.Xna.Framework;

namespace Side2D.Server.Physics;

public class NetworkPhysic : INetworkPhysic
{
    public event ServerUpdateBody? OnServerUpdateBody;

    public void ServerUpdatePosition(int index, EntityType type, Vector2 position, Vector2 velocity, 
        int rotation, bool includeSelf = false)
    {
        OnServerUpdateBody?.Invoke(index, type, position, velocity, rotation, includeSelf);
    }
}