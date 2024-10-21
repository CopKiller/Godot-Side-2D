using System.ComponentModel.DataAnnotations.Schema;
using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;

namespace Core.Game.Models.Player;

public class Position
{
    public int Id { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public Direction Direction { get; set; } = Direction.Right;
    
    [ForeignKey("PlayerModelId")]
    public int PlayerModelId { get; set; }
    
    public PlayerModel PlayerModel { get; set; }

    public Position(Vector2C vector2C)
    {
        X = vector2C.X;
        Y = vector2C.Y;
    }
    
    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Position() { }
    
    public void SetPosition(Position position)
    {
        X = position.X;
        Y = position.Y;
        Direction = position.Direction;
    }
    
    public void SetPosition(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    public override string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }
    
    public float DistanceTo(Position other)
    {
        return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }
}
