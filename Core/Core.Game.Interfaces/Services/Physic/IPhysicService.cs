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
    void AddWorldPhysic(int index, Vector2 gravity, List<CustomVertices> vertices, int density);
    void AddWorldPhysic(int index, Vector2 gravity);
    void AddPhysicToWorld(int index, List<CustomVertices> vertices, int density);
    void RemoveWorldPhysic(int index);
    void AddPhysicEntity(int index, int worldIndex, EntityType type, Vector2 position);
    void RemovePhysicEntity(int index, EntityType type);
    IPhysicEntity? GetPhysicEntity(int worldIndex, int entityIndex, EntityType type);
}