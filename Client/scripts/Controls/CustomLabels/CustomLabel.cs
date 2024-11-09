using System.Globalization;
using Godot;

namespace Side2D.scripts.Controls.CustomLabels;
    public partial class CustomLabel(
        double value,
        Vector2 relationalPosition,
        Node2D? nodePivot = null,
        Font? font = null,
        CustomLabelSize size = CustomLabelSize.Small,
        CustomLabelIncrease increase = CustomLabelIncrease.None,
        CustomLabelColor color = CustomLabelColor.White,
        CustomLabelIncrement increment = CustomLabelIncrement.None,
        CustomLabelTransparency transparency = CustomLabelTransparency.None)
        : Label
    {
        private double _value = value;
        private CustomLabelSize _size = size;
        private CustomLabelIncrease _increase = increase;
        private CustomLabelColor _color = color;
        private CustomLabelIncrement _increment = increment;
        private CustomLabelTransparency _transparency = transparency;

        private Tween? _tween;
        public Tween? Tween
        {
            get { return _tween ??= CreateTween(); }
        }

        public override void _Ready()
        {
            SetFontSize();
            
            ConfigureText();
            
            ApplyColorAndTransparency();

            if (nodePivot != null)
            {
                nodePivot.TreeExited += OnNodePivotTreeExited;
            }
            
            Position = relationalPosition;

            this.ZAsRelative = false;
            this.ZIndex = 100;
            
            _tween?.Play();
        }
        
        private void OnNodePivotTreeExited()
        {
            if (nodePivot == null) return;
            
            nodePivot.TreeExited -= OnNodePivotTreeExited;
            
            QueueFree();
        }
        public override void _ExitTree()
        {
            if (nodePivot != null)
            {
                nodePivot.TreeExited -= OnNodePivotTreeExited;
            }
            base._ExitTree();
        }

        private void SetFontSize()
        {
            if (font != null)
            {
                AddThemeFontOverride("font", font);
            }
            
            var fontSize = _size switch
            {
                CustomLabelSize.Small => 12,
                CustomLabelSize.Medium => 16,
                CustomLabelSize.Large => 20,
                CustomLabelSize.ExtraLarge => 40,
                _ => 12
            };
            
            AddThemeFontSizeOverride("font_size", fontSize);
        }

        private void ConfigureText()
        {
            var formattedValue = _increase switch
            {
                CustomLabelIncrease.None => _value.ToString(CultureInfo.InvariantCulture),
                CustomLabelIncrease.Percentage => $"{_value.ToString(CultureInfo.InvariantCulture)}%",
                CustomLabelIncrease.Currency => $"${_value.ToString(CultureInfo.InvariantCulture)}",
                _ => _value.ToString(CultureInfo.InvariantCulture)
            };

            Text = formattedValue;

            /*Text = _increment switch
            {
                CustomLabelIncrement.None => formattedValue,
                CustomLabelIncrement.Increase => $"+ {formattedValue}",
                CustomLabelIncrement.Decrease => $"- {formattedValue}",
                _ => formattedValue
            };*/
        }
        private void ApplyColorAndTransparency()
        {
            Modulate = _color switch
            {
                CustomLabelColor.Red => new Color(1, 0, 0),
                CustomLabelColor.Blue => new Color(0, 0, 1),
                CustomLabelColor.Green => new Color(0, 1, 0),
                CustomLabelColor.Yellow => new Color(1, 1, 0),
                CustomLabelColor.White => new Color(1, 1, 1),
                CustomLabelColor.Black => new Color(0, 0, 0),
                _ => new Color(1, 1, 1)
            };

            // Aplica a transparência com base na escolha do usuário.
            Modulate = _transparency switch
            {
                CustomLabelTransparency.None => Modulate,
                CustomLabelTransparency.Low => new Color(Modulate.R, Modulate.G, Modulate.B, 0.5f),
                CustomLabelTransparency.Medium => new Color(Modulate.R, Modulate.G, Modulate.B, 0.3f),
                CustomLabelTransparency.High => new Color(Modulate.R, Modulate.G, Modulate.B, 0.1f),
                _ => Modulate
            };
        }
    }