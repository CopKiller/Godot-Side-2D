
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
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
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
    
    public bool NeedUpdate { get; set; } = true;
    public int DefaultUpdateInterval { get; set; } = 1;
    public INetworkPhysic NetworkEvents { get; } = new NetworkPhysic();
    private Dictionary<int, PhysicWorld> Worlds { get; } = new();
    
    private float _lastUpdateTime;
    
    public void Register() { }

    public void Start()
    {
        // Inicializa o mundo de física para testes...
        //var physicWorld = new PhysicWorld(1, new Vector2(0, 9.8f));
        
        //Worlds.Add(1, physicWorld);
        
        //var playerPreFab = new PhysicPlayerDef(1, new Vector2(0, 0));
        
        //playerBodyTest = BodyFactory.CreateFromDef(physicWorld, playerPreFab);
        
        //FixtureFactory.AttachRectangle(32, 64, 1, Vector2.Zero, playerBodyTest);
        
        // Adicionar vários body, para analisar desempenho...
        
        //for (int i = 0; i < 1000; i++)
        //{
        //    var playerPreFab = new PhysicPlayerDef(i, new Vector2(0, 0));
        //
        //    var playerBody = BodyFactory.CreateFromDef(physicWorld, playerPreFab);
        
        //    FixtureFactory.AttachRectangle(32, 64, 1, Vector2.Zero, playerBody);
        //}
        
        
        //var player = new PhysicPlayer(1, playerBodyTest);
        
        //physicWorld.AddPhysicEntity(1, player);
    }

    public void Stop() {  }
    
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
        
        //Console.WriteLine($"{playerBodyTest?.Position}");
        
        //playerBodyTest?.ApplyForce(new Vector2(500,0));

        _lastUpdateTime = currentTime;
    }
    
    public void AddWorldPhysic(int index, Vector2 gravity, List<CustomVertices> vertices, int density)
    {
        var verticesList = vertices.Select(customVertices => new Vertices(customVertices)).ToList();

        Worlds.Add(index, new PhysicWorld(index, gravity, verticesList, density));
    }

    public void AddWorldPhysic(int worldIndex, Vector2 gravity)
    {
        var world = new PhysicWorld(worldIndex, gravity);
        
        Worlds.Add(worldIndex, world);
    }
    
    public void AddPhysicToWorld(int worldIndex, List<CustomVertices> vertices, int density)
    {
        var verticesList = vertices.Select(customVertices => new Vertices(customVertices)).ToList();
        
        Worlds[worldIndex].AddWorldPhysic(verticesList, density);
    }

    public void RemoveWorldPhysic(int worldIndex)
    {
        Worlds[worldIndex].RemoveWorldPhysic();
    }

    public void AddPhysicEntity(int worldIndex, int entityIndex, EntityType type, Vector2 position)
    {
        switch (type)
        {
            case EntityType.Player:
            {
                var result = Worlds[worldIndex].AddPhysicEntity(entityIndex, position);
            
                if (!result)
                {
                    throw new Exception("Failed to add physic entity to world, check if the world exists.");
                }
                
                break;
            }
            case EntityType.Monster:
            {
                var result = Worlds[worldIndex].AddPhysicEntity(entityIndex, position);
            
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

    public bool RemovePhysicEntity(int worldIndex, int entityIndex, EntityType type)
    {
        Worlds.TryGetValue(worldIndex, out var world);
        
        if (world == null)
        {
            throw new Exception("Failed to remove physic entity from world, check if the world exists.");
        }
        
        return world.RemovePhysicEntity(entityIndex);
    }

    public IPhysicEntity? GetPhysicEntity(int worldIndex, int entityIndex, EntityType type)
    {
        Worlds.TryGetValue(worldIndex, out var world);

        return world?.GetPhysicEntity(entityIndex);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        foreach (var value in Worlds.Values)
        {
            value.RemoveWorldPhysic();
        }
        
        Worlds.Clear();
    }
}