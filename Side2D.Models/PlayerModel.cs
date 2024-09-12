using Side2D.Models.Enum;
using Side2D.Models.Vectors;

namespace Side2D.Models;

public class PlayerModel
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public Gender Gender { get; set; }
    
    public Direction Direction { get; set; }
    
    public Vector2C Position { get; set; }

    public float JumpVelocity { get; set; } = -400.0F;
    
    public float Speed { get; set; } = 300.0F;
}