namespace Side2D.Models.Vectors;

public struct Vector2C(float x, float y)
{
    public float X { get; set; } = x;

    public float Y { get; set; } = y;
    
    public static Vector2C Zero => new(0, 0);
    
    public override string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }
}