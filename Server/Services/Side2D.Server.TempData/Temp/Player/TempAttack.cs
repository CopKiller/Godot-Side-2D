using Side2D.Server.TempData.Temp.Interface;

namespace Side2D.Server.TempData.Temp.Player;

public class TempAttack : ITempAttack
{
    private long _currentTick = 0;
    private const int AttackingSpeed = 1000; // 1 attack in one second
    public bool IsAttacking = false;
    public long LastAttackTime = 0;
    
    public void Update(long currentTick)
    {
        _currentTick = currentTick;
        if (IsAttacking && currentTick - LastAttackTime >= AttackingSpeed)
        {
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
    
    public void Dispose()
    {
        IsAttacking = false;
        LastAttackTime = 0;
    }
}