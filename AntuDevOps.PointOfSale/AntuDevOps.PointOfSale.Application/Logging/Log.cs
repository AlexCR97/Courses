namespace AntuDevOps.PointOfSale.Application.Logging;

public record Log(
    DateTimeOffset Timestamp,
    string Level,
    string MessageTemplate,
    string? Properties);

public interface ILogWriter
{
    Task WriteAsync(Log log);
}
