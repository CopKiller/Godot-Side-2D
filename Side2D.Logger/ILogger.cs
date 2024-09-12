namespace Side2D.Logger
{
    public interface ILogger
    {
        void Print(string message);
        void PrintInfo(string message);
        void PrintWarning(string message);
        void PrintError(string message);
    }
}
