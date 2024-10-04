using Side2D.Server.TempData.Interface;
using Side2D.Server.TempData.Temp.Interface;

namespace Side2D.Server.TempData.Temp;

public class TempAttack : ITempAttack
{
    private const int AttackingSpeed = 1000; // 1 attack in one second
    public bool IsAttacking;
    public long LastAttackTime;
    
    public void Update(long currentTick)
    {
        if (IsAttacking && currentTick - LastAttackTime >= AttackingSpeed)
        {
            IsAttacking = false;
        }
    }

    public bool CanAttack(long currentTick)
    {
        if (IsAttacking)
        {
            return false;
        }

        if (currentTick - LastAttackTime < AttackingSpeed)
        {
            return false;
        }

        LastAttackTime = currentTick;
        IsAttacking = true;

        return true;
    }
    
    public void Dispose()
    {
        IsAttacking = false;
        LastAttackTime = 0;
    }
}