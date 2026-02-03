namespace SMT.Domain.Exceptions;

public class NotFoundExceptions : Exception
{
    public NotFoundExceptions(string name, object key) :
        base($"Entity {key} was not found {name}")
    {
    }
}