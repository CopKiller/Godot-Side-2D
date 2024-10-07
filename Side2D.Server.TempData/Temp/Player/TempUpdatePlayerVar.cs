using Side2D.Server.TempData.Temp.Interface;

namespace Side2D.Server.TempData.Temp.Player;

public delegate void SendVitalsDelegate();
public class TempUpdatePlayerVar : ITempUpdatePlayerVar
{
    private long _currentTick = 0;
    private const int UpdateVitalsMs = 10000; // 10 seconds
    private long _lastVitalsUpdate = 0;
    public Action? SendVitals { get; set; }
    
    private bool _inCombat = false;
    private long _lastCombatTime = 0;
    private const int CombatTime = 30000; // 30 seconds

    public void SetPlayerCombat(bool inCombat)
    {
        _inCombat = inCombat;
        _lastCombatTime = _currentTick + CombatTime;
    }
    
    public void Update(long currentTick)
    {
        _currentTick = currentTick;

        if (_inCombat && _lastCombatTime < _currentTick)
        {
            _inCombat = false;
        }
        
        UpdateVitals();
    }
    
    private void UpdateVitals()
    {
        if (_inCombat) return;
        if (_lastVitalsUpdate >= _currentTick) return;
        _lastVitalsUpdate = _currentTick + UpdateVitalsMs;
        SendVitals?.Invoke();
    }
    
    
    public void Dispose()
    {
        SendVitals = null;
    }
}