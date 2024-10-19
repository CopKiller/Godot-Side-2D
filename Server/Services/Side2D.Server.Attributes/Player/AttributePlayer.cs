using Core.Game.Interfaces.Attribute.Player;
using Core.Game.Interfaces.Combat;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Side2D.Server.Attributes.Entity;

namespace Side2D.Server.Attributes.Player;

public class AttributePlayer(int index, Core.Game.Models.Player.Attributes attributes, Vitals vitals)
    : AttributeEntity, IAttributePlayer
{
    public override EntityType Type { get; } = EntityType.Player;
    public int Index { get; } = index;

    private long _currentTick = 0;
    private const int UpdateVitalsMs = 10000; // 10 seconds
    private long _lastVitalsUpdate = 0;
    
    public void TakeDamage(double damage)
    {
        vitals.TakeDamage(damage);
    }
    
    public double GetDamage()
    {
        return attributes.GetDamage();
    }
    
    public override void Update(long currentTick)
    {
        _currentTick = currentTick;
        
        UpdateVitals();
    }

    private void UpdateVitals()
    {
        if (_lastVitalsUpdate >= _currentTick) return;
        _lastVitalsUpdate = _currentTick + UpdateVitalsMs;
        
        // TODO: Implement vitals.RegenVitalsTemp() AND Process percents by attributes
        
        var hasUpdate = vitals.RegenVitalsTemp();
        if (!hasUpdate) return;
        
        vitals.NotifyVitalsChanged?.Invoke();
    }
    
    public override void Dispose()
    {
        
    }
}