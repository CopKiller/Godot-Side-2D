using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.Services.Physic.World;
using Core.Game.Models.Enum;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Shared;
using Infrastructure.Physics.Player;
using Microsoft.Xna.Framework;
using Side2D.Server.Physics.Entity;
using Side2D.Server.Physics.Player;

namespace Side2D.Server.Physics;

public class PhysicWorld : World, IPhysicWorld
{
    public readonly int Id;

    private readonly Dictionary<int, PhysicEntity> _physicEntities = [];
    
    public PhysicWorld(int id, Vector2 gravity) : base(gravity)
    {
        Id = id;
    }

    public PhysicWorld(int id, Vector2 gravity, List<Vertices> vertices, int density) : base(gravity)
    {
        Id = id;
        BodyFactory.CreateCompoundPolygon(this, vertices, density);
    }

    public void AddWorldPhysic(List<Vertices> vertices, int density)
    {
        BodyFactory.CreateCompoundPolygon(this, vertices, density);
    }

    public bool AddPhysicEntity(int index, Vector2 position)
    {
        var playerPreFab = new PhysicPlayerDef(index, position);
        
        var playerBody = BodyFactory.CreateFromDef(this, playerPreFab);
        
        FixtureFactory.AttachRectangle(32, 64, 1, Vector2.Zero, playerBody);
        
        var player = new PhysicPlayer(index, playerBody);
        
        _physicEntities.Add(index, player);
        
        return true;
    }
    
    public bool RemovePhysicEntity(int index)
    {
        if (!_physicEntities.TryGetValue(index, out var physic))
        {
            return false;
        }
        
        RemoveBody(physic.Body);
        return true;
    }

    public bool RemovePhysicEntity(IPhysicEntity physicEntity)
    {
        if (!_physicEntities.TryGetValue(physicEntity.Index, out var entity))
        {
            return false;
        }

        if (entity.Type != physicEntity.Type) return false;
        
        RemoveBody(entity.Body);
        _physicEntities.Remove(physicEntity.Index);
        return true;

    }
    
    public PhysicEntity GetPhysicEntity(int index)
    {
        return _physicEntities[index];
    }
    
    public void RemoveWorldPhysic()
    {
        Clear();
    }
}