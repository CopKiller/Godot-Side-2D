using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Side2D.Server.Physics.Entity;

namespace Side2D.Server.Physics.Player;

public partial class PhysicPlayer
{
    public bool IsAttacking = false;
    public long LastAttackTime = 0;

    private const int AttackingSpeed = 1000; // 1 attack in one second
    private const int Range = 64;
    
    private void UpdateAttack()
    {
        if (IsAttacking && _currentTick - LastAttackTime >= AttackingSpeed)
        {
            CheckAttack();
            IsAttacking = false;
        }
    }

    private void CheckAttack()
    {
        // Buscar jogadores no range de ataque
        var playersInRadius = physicService.GetPlayersInRadius(playerModel.Position, Range);
        
        foreach (var player in playersInRadius)
        {
            if (player == null) continue;
            if (player.Index == Index) continue;
            physicService.NotifyCombatService(Index, player.Index);
            
            player.ApplyKnockback(playerModel.Position, 1.0f);
            
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
}