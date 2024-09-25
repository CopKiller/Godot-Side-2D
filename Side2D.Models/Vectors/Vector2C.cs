using System.ComponentModel.DataAnnotations.Schema;

namespace Side2D.Models.Vectors;

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
    
    [ForeignKey("PlayerModelId")]
    public int PlayerModelId { get; set; }
    
    public PlayerModel PlayerModel { get; set; }
    
    public static Vector2C Zero => new(0,0);
    
    public override string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }
}