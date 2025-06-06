namespace API;

public class ContructorExceptions : Exception
{
    public ContructorExceptions(string message) : base(message)
    {
    }

    public ContructorExceptions() : base()
    {
    }

    public ContructorExceptions(string message, Exception ex) : base(message, ex)
    {
    }
 }