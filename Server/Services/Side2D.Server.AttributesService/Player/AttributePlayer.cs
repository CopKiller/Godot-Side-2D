using Core.Game.Interfaces.Attribute.Player;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Side2D.Server.AttributesService.Entity;

namespace Side2D.Server.AttributesService.Player;

public class AttributePlayer(int index, IStats iStats, Vitals vitals)
    : AttributeEntity(index, EntityType.Player) , IAttributePlayer
{

    private long _currentTick = 0;
    private const int UpdateVitalsMs = 10000; // 10 seconds
    private long _lastVitalsUpdate = 0;
    
    private bool _inCombat = false;
    
    public void TakeDamage(double damage)
    {
        vitals.TakeDamage(damage);
        
        networkEvents.ServerUpdateVitals(Index);
        
        networkEvents.ServerVitalsNotify(Index, VitalsType.Health, -damage);
    }
    
    public double GetDamage()
    {
        return iStats.GetDamage();
    }
    
    public void SetCombatState(bool inCombat)
    {
        _inCombat = inCombat;
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
        
        if (_inCombat) return;
        
        var hasUpdate = vitals.RegenVitalsTemp();
        if (!hasUpdate) return;
        
        networkEvents.ServerUpdateVitals(Index);
    }
    
    public override void Dispose()
    {
        
    }
}