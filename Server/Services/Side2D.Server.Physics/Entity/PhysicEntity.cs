using System.Diagnostics;
using System.Numerics;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;

namespace Side2D.Server.Physics.Entity;

public class PhysicEntity(int index, EntityType type, Position position, INetworkPhysic networkPhysic) : IPhysicEntity
{
    public EntityType Type { get; } = type;
    public int Index { get; } = index;
    private Position Position { get; } = position;
    private VectorTwo Velocity { get; set; } = VectorTwo.Zero;

    private long _lastTick = 0;
    private long _lastUpdatePositionTick = 0;
    
    public virtual void Move(VectorTwo newPosition, bool isServer = false)
    {
        Position.SetValues(newPosition);
        networkPhysic.ServerUpdatePosition(Index, Type, isServer);
    }
    
    public void ApplyKnockback(Position attackerPosition, float impactForce)
    {
        /*Console.WriteLine($"Entity {Index} received knockback from {attackerPosition} with force {impactForce}");
        
        Console.WriteLine($"Current velocity: {Velocity}");
        Console.WriteLine($"Current position: {Position}");*/
        
        var pos = new Position();
        pos.SetValues(Position);
        
        var direction = (pos - attackerPosition).Normalize();
        Velocity += direction * impactForce;
        
        //Console.WriteLine($"New velocity: {Velocity}");

        // Limitar velocidade máxima
        const float maxVelocity = 10.0f;
        if (Velocity.LengthSquared() > maxVelocity * maxVelocity)
        {
            Velocity = Velocity.Normalize() * maxVelocity;
        }
        
        //Console.WriteLine($"Limited velocity: {Velocity}");
    }
    
    private void UpdatePosition()
    {
        if (Velocity == VectorTwo.Zero) return;

        if (_lastUpdatePositionTick > _lastTick) return;
        
        //Console.WriteLine($"Updating position for entity {Index} with velocity {Velocity}");
        
        //Console.WriteLine($"Current position: {Position}");
        
        // Atualizar a posição e verificar colisão (placeholder para futura implementação)
        var newPosition = Position + Velocity;
        if (IsCollisionFree(newPosition))
        {
            Position.Increment(Velocity);
            
            networkPhysic.ServerUpdateKnockback(Index, Type, true, Position);
        }
        
        //Console.WriteLine($"New position: {Position}");

        // Aplicar damping dinâmico
        const float damping = 0.9f;
        Velocity *= damping;
        
        //Console.WriteLine($"New velocity: {Velocity}");

        if (Velocity.LengthSquared() < 0.01f)
        {
            Velocity.Reset();
        }

        _lastUpdatePositionTick = _lastTick + 15;
    }

    private bool IsCollisionFree(VectorTwo newPosition)
    {
        // Placeholder para sistema de colisão
        return true;
    }
    
    
    
    public virtual void Update(long currentTick)
    {
        _lastTick = currentTick;
        
        UpdatePosition();
    }

    public virtual void Dispose()
    {
        
    }
}