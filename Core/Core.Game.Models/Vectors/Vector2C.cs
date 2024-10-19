using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Game.Models.Vectors;

public class Vector2C
{
    public Vector2C(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    public Vector2C() { }
    
    public int Id { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    
    public static Vector2C Zero => new(0,0);
    
    public override string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }
    
    public float DistanceTo(Vector2C other)
    {
        return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }
    
    public void SetValues(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    public void SetValues(Vector2C vector)
    {
        X = vector.X;
        Y = vector.Y;
    }
}