using System.Collections.Generic;
using Godot;

namespace Side2D.scripts.Controls.Fonts;

public class FontManager
{
    private const string FontPath = "res://resources/Fonts/";
    
    public static Font LoadFont(string fontName)
    {
        var font = GD.Load<FontFile>(FontPath + fontName);
        return font;
    }
}