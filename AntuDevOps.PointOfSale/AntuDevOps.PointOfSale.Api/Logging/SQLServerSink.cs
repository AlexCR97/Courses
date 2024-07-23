using AntuDevOps.PointOfSale.Application.Logging;
using Serilog.Core;
using Serilog.Events;
using System.Text.Json.Nodes;

namespace AntuDevOps.PointOfSale.Api.Logging;

internal class SQLServerSink : ILogEventSink
{
    private readonly IServiceProvider _serviceProvider;

    public SQLServerSink(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        Task.Run(async () =>
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var logWriter = scope.ServiceProvider.GetRequiredService<ILogWriter>();

                var jsonProperties = logEvent.Properties
                    .ToDictionary()
                    .Select(x => new KeyValuePair<string, JsonNode?>(
                        x.Key,
                        x.Value?.ToString()));

                var jsonData = new JsonObject(jsonProperties).ToJsonString();

                await logWriter.WriteAsync(new Log(
                    logEvent.Timestamp,
                    logEvent.Level.ToString(),
                    logEvent.MessageTemplate.Text,
                    jsonData));
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error(ex, "Unable to write to SQL Server Sink");
            }
        });
    }
}
