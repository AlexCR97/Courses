using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AntuDevOps.PointOfSale.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogWarning("An error occurred :(");

        context.Result = new JsonResult(new
        {
            Code = context.Exception.GetType().Name,
            Message = context.Exception.Message,
        })
        {
            StatusCode = 400,
        };
    }
}