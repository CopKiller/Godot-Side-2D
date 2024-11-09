using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Xna.Framework;

namespace Core.Game.Models.Vectors;

public class VectorTwo
{
    public int Id { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    
    public VectorTwo() { }
    public VectorTwo(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    public override string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }
    
    // Implementação de Equals para comparar dois VectorTwo
    public override bool Equals(object? obj)
    {
        if (obj is VectorTwo other)
        {
            return Math.Abs(X - other.X) < 0.0001f && Math.Abs(Y - other.Y) < 0.0001f;
        }
        return false;
    }

    protected bool Equals(VectorTwo other)
    {
        return Id == other.Id && X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
    
    public static VectorTwo Zero => new(0,0);
    
    public float DistanceTo(VectorTwo other)
    {
        return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }
    
    public VectorTwo Normalize()
    {
        var s = this.LengthSquared();
        if (s == 0.0)
        {
            this.X = this.Y = 0.0f;
        }
        else
        {
            var num = MathF.Sqrt(s);
            this.X /= num;
            this.Y /= num;
        }
        
        return this;
    }
    
    public static VectorTwo operator +(VectorTwo a, VectorTwo b)
    {
        a.X += b.X;
        a.Y += b.Y;
        return a;
    }
    
    public static VectorTwo operator -(VectorTwo a, VectorTwo b)
    {
        a.X -= b.X;
        a.Y -= b.Y;
        return a;
    }
    
    public static VectorTwo operator *(VectorTwo a, VectorTwo b)
    {
        a.X *= b.X;
        a.Y *= b.Y;
        return a;
    }
    
    public static VectorTwo operator *(VectorTwo a, float b)
    {
        a.X *= b;
        a.Y *= b;
        return a;
    }
    
    public static bool operator ==(VectorTwo a, float b)
    {
        return Math.Abs(a.X - b) < 0.001F && Math.Abs(a.Y - b) < 0.001F;
    }

    public static bool operator !=(VectorTwo a, float b) => !(a == b);
    
    public static bool operator ==(VectorTwo a, VectorTwo b)
    {
        return Math.Abs(a.X - b.X) < 0.001F &&
               Math.Abs(a.Y - b.Y) < 0.001F;
    }

    public static bool operator !=(VectorTwo a, VectorTwo b) => !(a == b);
    
    public float LengthSquared()
    {
        return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
    }
    
    public void SetValues(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    public void SetValues(VectorTwo vector)
    {
        X = vector.X;
        Y = vector.Y;
    }
    
    public void SetValues(Vector2 vector)
    {
        X = vector.X;
        Y = vector.Y;
    }
    
    public void Reset()
    {
        X = 0F;
        Y = 0F;
    }
}