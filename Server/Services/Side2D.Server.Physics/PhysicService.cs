
using System.Diagnostics;
using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Interfaces.Services.Physic.World;
using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Genbox.VelcroPhysics.Shared;
using Infrastructure.Physics;
using Infrastructure.Physics.Player;
using Microsoft.Xna.Framework;
using Side2D.Server.Physics.Entity;
using Side2D.Server.Physics.Player;
using VelcroPhysics.Models.Shared;

namespace Side2D.Server.Physics;

public class PhysicService(ICombatService combatService) : IPhysicService
{
    // TODO: Implement physics service
    
    public int DefaultUpdateInterval { get; set; } = 1;
    public INetworkPhysic NetworkEvents { get; } = new NetworkPhysic();
    
    private Dictionary<int, IPhysicWorld> Worlds { get; } = new();
    
    private float _lastUpdateTime;
    
    public void Register()
    {
        
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        Dispose();
    }

    public void Restart()
    {
        Dispose();
        Start();
    }

    public void Update(long currentTick)
    {
        // Calcula o deltaTime com base no tempo decorrido
        var currentTime = currentTick / 1000f;
        var deltaTime = currentTime - _lastUpdateTime;
        
        // Atualiza o mundo de física buscando todos do dictionary
        foreach (var value in Worlds.Values)
        {
            value.Step(deltaTime);
        }
        
        _lastUpdateTime = currentTime;
    }
    
    public void AddWorldPhysic(int index, Vector2 gravity, List<CustomVertices> vertices, int density)
    {
        
        var verticesList = new List<Vertices>();
        foreach (var customVertices in vertices)
        {
            verticesList.Add(new Vertices(customVertices));
        }
        
        Worlds.Add(index, new PhysicWorld(index, gravity, verticesList, density));
    }

    public void AddWorldPhysic(int index, Vector2 gravity)
    {
        
    }
    
    public void AddPhysicToWorld(int index, List<CustomVertices> vertices, int density)
    {
        
    }

    public void RemoveWorldPhysic(int index)
    {
        
    }

    public void AddPhysicEntity(int index, int worldIndex, EntityType type, Vector2 position)
    {
        switch (type)
        {
            case EntityType.Player:
            {
                var result = Worlds[worldIndex].AddPhysicEntity(index, position);
            
                if (!result)
                {
                    throw new Exception("Failed to add physic entity to world, check if the world exists.");
                }
                
                break;
            }
            case EntityType.Monster:
            {
                var result = Worlds[worldIndex].AddPhysicEntity(index, position);
            
                if (!result)
                {
                    throw new Exception("Failed to add physic entity to world, check if the world exists.");
                }

                break;
            }
            case EntityType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void RemovePhysicEntity(int index, EntityType type)
    {
        
    }

    public IPhysicEntity? GetPhysicEntity(int worldIndex, int entityIndex, EntityType type)
    {
        Worlds.TryGetValue(worldIndex, out var world);
        
        if (world == null)
        {
            return null;
        }
        
        if (world is PhysicWorld physicWorld)
        {
            return physicWorld.GetPhysicEntity(entityIndex);
        }
        
        return null;
    }
    
    public void Dispose()
    {
        foreach (var value in Worlds.Values)
        {
            if (value is PhysicWorld disposable)
            {
                disposable.RemoveWorldPhysic();
            }
        }
        
        Worlds.Clear();
    }
}