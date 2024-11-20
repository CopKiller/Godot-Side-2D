using System.Diagnostics;
using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Side2D.Server.PhysicsService.Entity;

namespace Side2D.Server.PhysicsService.Player;

public class PhysicPlayer : PhysicEntity, IPhysicPlayer
{
    public readonly Body PlayerBody;
    
    public PhysicPlayer(int index, Body body) : base(index, EntityType.Player, body)
    {
        PlayerBody = body;
    }
}