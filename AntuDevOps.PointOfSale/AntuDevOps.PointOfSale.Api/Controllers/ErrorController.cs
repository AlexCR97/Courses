using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace AntuDevOps.PointOfSale.Api.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleError()
    {
        _logger.LogWarning("An error occurred :(");

        var exceptionHandlerFeature = HttpContext.Features.GetRequiredFeature<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature.Error;

        return BadRequest(new
        {
            Code = exception.GetType().Name,
            Message = exception.Message,
        });
    }
}
