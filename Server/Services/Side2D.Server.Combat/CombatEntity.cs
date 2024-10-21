using Core.Game.Interfaces.Combat;
using Core.Game.Models.Enum;

namespace Side2D.Server.Combat;

public class CombatEntity : ICombatEntity
{
    public virtual EntityType Type { get; } = EntityType.None;
    
    private long _currentTick = 0;
    
    public bool _inCombat = false;
    private long _lastCombatTime = 0;
    private const int CombatTime = 30000; // 30 seconds
    
    public Action<bool> NotifyCombatStateChanged { get; set; }
    
    public bool GetCombatState()
    {
        return _inCombat;
    }
    
    public void SetCombatState(bool inCombat)
    {
        _inCombat = inCombat;
        
        if (_inCombat)
            _lastCombatTime = _currentTick + CombatTime;
        
        NotifyCombatStateChanged?.Invoke(_inCombat);
    }

    public virtual void Update(long currentTick)
    {
        _currentTick = currentTick;

        if (_inCombat && _lastCombatTime < _currentTick)
        {
            _inCombat = false;
            NotifyCombatStateChanged?.Invoke(_inCombat);
        }
    }

    public virtual void Dispose()
    {
        
    }
}