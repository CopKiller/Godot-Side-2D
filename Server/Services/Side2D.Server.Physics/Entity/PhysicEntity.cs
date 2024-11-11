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

namespace Side2D.Server.Physics.Entity;

public class PhysicEntity : IPhysicEntity
{
    public int Index { get; }
    public EntityType Type { get; }
    public readonly Body Body;
    
    protected PhysicEntity(int index, EntityType type, Body body)
    {
        Index = index;
        Type = type;
        Body = body;
    }
    
    public void Move(Vector2 velocity, bool isServer = false)
    {
        Body.ApplyForce(velocity);
    }
    
    public void Update(long currentTick) { }
    
    public void Dispose() { }
}