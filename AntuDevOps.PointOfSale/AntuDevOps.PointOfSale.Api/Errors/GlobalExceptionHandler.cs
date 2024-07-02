using AntuDevOps.AspNetCore.Http.Problems;
using Microsoft.AspNetCore.Diagnostics;

namespace AntuDevOps.PointOfSale.Api.Errors;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IProblemDetailsResolver _problemDetailsResolver;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsResolver problemDetailsResolver)
    {
        _logger = logger;
        _problemDetailsResolver = problemDetailsResolver;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogWarning("An error occurred :(");

        var problemDetails = _problemDetailsResolver.Resolve(exception);

        httpContext.Response.StatusCode = problemDetails.Status ?? 500;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
