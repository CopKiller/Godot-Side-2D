namespace Side2D.Models.Result;

public class ModelException(string msg)
{
    public bool IsError { get; } = true;
    public string Message { get; } = msg;
    
    public override string ToString()
    {
        return Message;
    }
}