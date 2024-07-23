namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class LogEntity
{
    public LogEntity()
    {
    }

    public LogEntity(int id, DateTimeOffset timestamp, string level, string messageTemplate, string? properties)
    {
        Id = id;
        Timestamp = timestamp;
        Level = level;
        MessageTemplate = messageTemplate;
        Properties = properties;
    }

    public int Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Level { get; set; }
    public string MessageTemplate { get; set; }
    public string? Properties { get; set; }
}