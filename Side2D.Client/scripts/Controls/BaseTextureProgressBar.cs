using Godot;

namespace Side2D.scripts.Controls;

public partial class BaseTextureProgressBar : TextureProgressBar
{
    private Label _lblValue;
    
    private double _currentValue;
    private double _maxValue;
    private bool _isAnimating;
    private bool _isPercent;
    
    public double CurrentValue
    {
        get => _currentValue;
        set => CallDeferred(nameof(SetCurrentValue), value);
    }

    public new double MaxValue
    {
        get => _maxValue;
        set => CallDeferred(nameof(SetMaxValue), value);
    }

    
    public override void _Ready()
    {
        _lblValue = GetNode<Label>("lblValue");
    }
    
    public void SetOptions(bool isPercent, bool isAnimating)
    {
        _isPercent = isPercent;
        _isAnimating = isAnimating;
    }
    
    private void SetMaxValue(int value)
    {
        _maxValue = value;
    }
    
    private void SetCurrentValue(double value)
    {
        _currentValue = value;

        if (_isPercent)
        {
            value = (value / _maxValue) * 100;
            _lblValue.Text = $"{value}%";
        } else
        {
            if (value > _maxValue)
            {
                value = _maxValue;
            }
            
            _lblValue.Text = $"{value}";
        }
        
        if (_isAnimating)
        {
            // create a tween to animate the value change
            var tween = CreateTween();
            tween.TweenProperty(this, "value", value, 0.5f);
            tween.SetTrans(Tween.TransitionType.Elastic);
            tween.SetEase(Tween.EaseType.InOut);
            tween.Play();
        } else
        {
            Value = value;
        }
        
    }
}