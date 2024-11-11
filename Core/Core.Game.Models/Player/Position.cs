using System.ComponentModel.DataAnnotations.Schema;
using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;
using Microsoft.Xna.Framework;

namespace Core.Game.Models.Player;

public class Position : VectorTwo
{
    public Direction Direction { get; set; } = Direction.Right;
    
    [ForeignKey("PlayerModelId")]
    public int PlayerModelId { get; set; }
    public PlayerModel PlayerModel { get; set; }
    
    [NotMapped] public int Index { get; set; }
    [NotMapped] public bool IsMoving { get; set; } = false;
    [NotMapped] public Vector2 Velocity { get; set; } = Vector2.Zero;
    
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
    
    public void SetValues(Vector2 position)
    {
        X = position.X;
        Y = position.Y;
    }

    public Vector2 GetVector2()
    {
        return new Vector2(X, Y);
    }
}
