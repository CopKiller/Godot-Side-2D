using System.ComponentModel.DataAnnotations.Schema;
using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;

namespace Core.Game.Models.Player;

public class Position : VectorTwo
{
    public Direction Direction { get; set; } = Direction.Right;
    
    [ForeignKey("PlayerModelId")]
    public int PlayerModelId { get; set; }
    
    public PlayerModel PlayerModel { get; set; }
    
    [NotMapped] public int Index { get; set; }
    [NotMapped] public bool IsMoving { get; set; } = false;
    [NotMapped] public VectorTwo Velocity { get; set; } = VectorTwo.Zero;
    
    public Position(float x, float y, Direction direction)
    {
        X = x;
        Y = y;
        Direction = direction;
    }

    public Position() { }
    
    public void SetValues(Position position)
    {
        X = position.X;
        Y = position.Y;
        Direction = position.Direction;
        Index = position.Index;
        IsMoving = position.IsMoving;
        Velocity = position.Velocity;
    }
    
    public void SetValues(float x, float y, Direction direction)
    {
        X = x;
        Y = y;
        Direction = direction;
    }
    
    /*public override string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }*/
    
    /*public float DistanceTo(Position other)
    {
        return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }*/
}
