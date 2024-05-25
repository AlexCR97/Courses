namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal abstract class BaseEntity
{
    protected BaseEntity()
    {
    }

    protected BaseEntity(int id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        LastModifiedAt = lastModifiedAt;
        LastModifiedBy = lastModifiedBy;
    }

    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}
