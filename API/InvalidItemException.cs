namespace API;

public class InvalidItemException : Exception
{
    public InvalidItemException(string message) : base(message)
    {
        
    }
}