using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.Service;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Microsoft.Xna.Framework;
using VelcroPhysics.Models.Shared;

namespace Core.Game.Interfaces.Physic;

public interface IPhysicService : ISingleService
{
    INetworkPhysic NetworkEvents { get; }
    int DefaultUpdateInterval { get; set; }
    void AddWorldPhysic(int worldIndex, Vector2 gravity, List<CustomVertices> vertices, int density);
    void AddWorldPhysic(int worldIndex, Vector2 gravity);
    void AddPhysicToWorld(int worldIndex, List<CustomVertices> vertices, int density);
    void RemoveWorldPhysic(int worldIndex);
    void AddPhysicEntity(int worldIndex, int entityIndex, EntityType type, Vector2 position);
    bool RemovePhysicEntity(int worldIndex, int entityIndex, EntityType type);
    IPhysicEntity? GetPhysicEntity(int worldIndex, int entityIndex, EntityType type);
}