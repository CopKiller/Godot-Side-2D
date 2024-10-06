using Side2D.Models.Vectors;

namespace Side2D.Models.Player;

public class Position : Vector2C
{
    public Position(Vector2C vector2C) : base(vector2C.X, vector2C.Y)
    {
        
    }

    public Position()
    {
        
    }
    
    public void SetPosition(Vector2C? vector2C)
    {
        X = vector2C.X;
        Y = vector2C.Y;
    }
}
