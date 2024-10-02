namespace Side2D.Extension;

public static class ClassExtensions
{
    public static string UniqueAccess<T>(this T cls) where T : class
    {
        return "%" + cls.GetType().Name;
    }
}