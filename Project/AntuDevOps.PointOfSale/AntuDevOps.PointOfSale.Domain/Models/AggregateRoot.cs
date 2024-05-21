namespace AntuDevOps.PointOfSale.Domain.Models;

public abstract class AggregateRoot<TId>
{
    protected AggregateRoot(TId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        LastModifiedAt = lastModifiedAt;
        LastModifiedBy = lastModifiedBy;
    }

    public TId Id { get; }
    public DateTime CreatedAt { get; }
    public string CreatedBy { get; }
    public DateTime? LastModifiedAt { get; protected set; }
    public string? LastModifiedBy { get; protected set; }
}
