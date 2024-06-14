using Microsoft.AspNetCore.Diagnostics;

namespace AntuDevOps.PointOfSale.Api.Errors;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IErrorResponseParser _errorResponseParser;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IErrorResponseParser errorResponseParser)
    {
        _logger = logger;
        _errorResponseParser = errorResponseParser;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogWarning("An error occurred :(");

        var errorResponse = _errorResponseParser.Parse(exception);

        httpContext.Response.StatusCode = (int)errorResponse.Status;

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}
