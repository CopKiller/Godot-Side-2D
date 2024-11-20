using Core.Game.Models.Enum;
using Core.Game.Models.Interfaces;

namespace Side2D.Server.AttributesService;

public class VitalsService<T> where T : IVitals
{
    private readonly T _vitals;
    
    public bool IsDead => _vitalMap[VitalsType.Health].GetCurrent() <= 0;

    private readonly Dictionary<VitalsType, Vital> _vitalMap;
    
    private const string InvalidVitalMessage = "Tipo de vital inválido.";
    
    public VitalsService(T vitals)
    {
        _vitals = vitals;
        _vitalMap = new Dictionary<VitalsType, Vital>
        {
            [VitalsType.Health] = new Vital(() => vitals.Health, value => vitals.Health = value, () => vitals.MaxHealth, value => vitals.MaxHealth = value),
            [VitalsType.Mana] = new Vital(() => vitals.Mana, value => vitals.Mana = value, () => vitals.MaxMana, value => vitals.MaxMana = value),
        };
    }
    
    private Vital GetVital(VitalsType vitalType)
    {
        if (!_vitalMap.TryGetValue(vitalType, out var vital))
            throw new ArgumentOutOfRangeException(nameof(vitalType), vitalType, InvalidVitalMessage);

        return vital;
    }

    public bool ModifyVital(VitalsType vitalType, double value)
    {
        var vital = GetVital(vitalType);
        var current = vital.GetCurrent();
        var max = vital.GetMax();
        var newValue = Math.Clamp(current + value, 0, max);

        if (Math.Abs(current - newValue) < 0.0001)
            return false;

        vital.SetCurrent(newValue);
        return true;
    }

    public bool ModifyMaxVital(VitalsType vitalType, double value)
    {
        var vital = GetVital(vitalType);
        var current = vital.GetCurrent();
        var max = vital.GetMax();
        var newMax = Math.Max(max + value, 0);
        var newCurrent = Math.Clamp(current, 0, newMax);
        
        if (newMax < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "O valor máximo não pode ser negativo.");

        if (Math.Abs(max - newMax) < 0.0001)
            return false;

        vital.SetMax(newMax);
        vital.SetCurrent(newCurrent);
        return true;
    }

    public bool UpdateVitals(T vitals)
    {
        var updated = false;

        foreach (var (vitalType, vital) in _vitalMap)
        {
            var current = vital.GetCurrent();
            var max = vital.GetMax();
            var clampedValue = Math.Clamp(current, 0, max);

            if (!(Math.Abs(current - clampedValue) > 0.0001)) continue;
            
            vital.SetCurrent(clampedValue);
            updated = true;
        }

        return updated;
    }
    
    private class Vital(Func<double> getCurrent, Action<double> setCurrent, Func<double> getMax, Action<double> setMax)
    {
        public Func<double> GetCurrent { get; } = getCurrent;
        public Action<double> SetCurrent { get; } = setCurrent;
        public Func<double> GetMax { get; } = getMax;
        public Action<double> SetMax { get; } = setMax;
    }
}