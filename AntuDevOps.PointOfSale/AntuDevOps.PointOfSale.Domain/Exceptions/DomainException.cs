namespace AntuDevOps.PointOfSale.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string code)
    {
        Code = code;
    }

    public DomainException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    public DomainException(string code, string message, Exception? innerException)
        : base(message, innerException)
    {
        Code = code;
    }

    public string Code { get; }
}