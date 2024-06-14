namespace AntuDevOps.PointOfSale.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string resourceName, object resourceIdentifier)
        : base("NotFound", "Resource was not found")
    {
        ResourceName = resourceName;
        ResourceIdentifier = resourceIdentifier;
    }

    public string ResourceName { get; }
    public object ResourceIdentifier { get; }
}