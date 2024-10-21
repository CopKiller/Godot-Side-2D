using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Side2D.Server.Physics.Entity;

namespace Side2D.Server.Physics.Player;

public class PhysicAttack(int index) : PhysicEntity, IPhysicAttack
{
    private long _currentTick = 0;
    private const int AttackingSpeed = 1000; // 1 attack in one second
    public bool IsAttacking = false;
    public long LastAttackTime = 0;
    
    private int Range = 64;
    
    public Action<int, int>? FinishAttack { get; set; }
    
    public override void Update(long currentTick)
    {
        _currentTick = currentTick;
        if (IsAttacking && currentTick - LastAttackTime >= AttackingSpeed)
        {
            FinishAttack?.Invoke(index, Range);
            IsAttacking = false;
        }
    }

    public bool CanAttack()
    {
        if (IsAttacking)
        {
            return false;
        }

        if (_currentTick - LastAttackTime < AttackingSpeed)
        {
            // Not enough time to attack again
            return false;
        }

        LastAttackTime = _currentTick;
        IsAttacking = true;

        return true;
    }
    
    public override void Dispose()
    {
        IsAttacking = false;
        LastAttackTime = 0;
    }
}