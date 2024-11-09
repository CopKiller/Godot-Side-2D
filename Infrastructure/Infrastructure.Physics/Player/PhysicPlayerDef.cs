using Genbox.VelcroPhysics.Dynamics;
using Infrastructure.Physics.Entity;
using Microsoft.Xna.Framework;

namespace Infrastructure.Physics.Player;

public class PhysicPlayerDef : PhysicEntityDef
{
    public PhysicPlayerDef(int index, Vector2 position)
    {
        Type = BodyType.Dynamic;
        Position = position;
        UserData = index;
    }
}