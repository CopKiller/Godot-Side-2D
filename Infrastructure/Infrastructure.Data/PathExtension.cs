namespace Infrastructure.Data;

public static class PathExtension
{
    public static string ToIndex(this string path, int index)
    {
        return path.Replace("@", index.ToString());
    }
}