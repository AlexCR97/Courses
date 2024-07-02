namespace AntuDevOps.PointOfSale.Api.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("An error occurred :(");

            context.Response.StatusCode = 400;

            await context.Response.WriteAsJsonAsync(new
            {
                Code = ex.GetType().Name,
                Message = ex.Message,
            });
        }
    }
}