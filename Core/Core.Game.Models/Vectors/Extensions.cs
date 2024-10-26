namespace Core.Game.Models.Vectors;

public static class Extension
{
    public static T Increment<T>(this T a, T b) where T : VectorTwo
    {
        a.X += b.X;
        a.Y += b.Y;
        return a;
    }
}