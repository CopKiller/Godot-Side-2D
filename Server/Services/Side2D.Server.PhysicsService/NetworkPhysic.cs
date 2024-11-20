using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;
using Microsoft.Xna.Framework;

namespace Side2D.Server.PhysicsService;

public class NetworkPhysic : INetworkPhysic
{
    public ServerUpdateBody? OnServerUpdateBody { get; set; }
}