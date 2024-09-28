using Godot;

namespace Side2D.scripts.Controls;

public partial class BaseWindow : Window
{
    // O godot não suporta adicionar icones ao componente Window, ele usa Window native do sistema operacional
    //[Export] public Texture2D Icon { get; set; }
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //LoadIcon();
        
        AssignSignals();
    }
    
    /*private void LoadIcon()
    {
        if (Icon == null)
            return;

        var icon = new TextureRect
        {
            Texture = Icon,
            StretchMode = TextureRect.StretchModeEnum.Scale,
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter,
            
        };


        // Ajuste o tamanho e a posição com as novas propriedades
        icon.Size = new Vector2(16, 16);  // Define o tamanho do ícone
        icon.Position = new Vector2(5, 5);  // Define a posição do ícone

        AddChild(icon);
    }*/

    private void AssignSignals()
    {
        this.CloseRequested += CloseWindow;
    }

    public void CloseWindow()
    {
        this.Hide();
    }

    public void ShowWindow()
    {
        this.Show();
    }
}