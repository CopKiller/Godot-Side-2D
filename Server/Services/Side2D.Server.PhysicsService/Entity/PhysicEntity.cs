using System.Diagnostics;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Interfaces.Services.Physic.World;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using Genbox.VelcroPhysics.Definitions;
using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace Side2D.Server.PhysicsService.Entity;

public class PhysicEntity : IPhysicEntity
{
    public int Index { get; }
    public EntityType Type { get; }
    public readonly Body Body;
    
    public Vector2 CurrentPosition { get; private set; }
    public float CurrentRotation { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    
    protected PhysicEntity(int index, EntityType type, Body body)
    {
        Index = index;
        Type = type;
        Body = body;
        
        CurrentPosition = body.Position;
        CurrentRotation = body.Rotation;
        CurrentVelocity = body.LinearVelocity;
    }
    
    public void Move(Vector2 velocity, bool isServer = false)
    {
        Body.ApplyForce(velocity);
    }

    public bool Update()
    {
        if (Body.Position == CurrentPosition &&
            Math.Abs(Body.Rotation - CurrentRotation) < 0.005 &&
            Body.LinearVelocity == CurrentVelocity) 
            return false;
        
        CurrentPosition = Body.Position;
        CurrentRotation = Body.Rotation;
        CurrentVelocity = Body.LinearVelocity;
            
        Debug.WriteLine($"Entity {Index} moved to {CurrentPosition}");
        
        return true;
    }
    
    public void Update(long currentTick) { }
    
    public void Dispose() { }
}