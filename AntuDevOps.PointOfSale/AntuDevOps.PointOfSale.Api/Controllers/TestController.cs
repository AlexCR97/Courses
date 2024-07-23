using Microsoft.AspNetCore.Mvc;

namespace AntuDevOps.PointOfSale.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public OkResult RespondOk()
    {
        return Ok();
    }

    [HttpGet("logs")]
    public void Logs()
    {
        _logger.LogTrace("This is a trace message");
        _logger.LogDebug("This is a debug message");
        _logger.LogInformation("This is an information message");
        _logger.LogWarning("This is a warning message");
        _logger.LogError("This is an error message");
        _logger.LogCritical("This is a critical message");
    }

    [HttpGet("logs/structured/1")]
    public void LogsStructured1()
    {
        // Unstructured
        _logger.LogInformation($"The current time is {DateTime.UtcNow}");

        // Structured
        _logger.LogInformation("The current time is {Now}", DateTime.UtcNow);
    }

    [HttpGet("logs/structured/2")]
    public void LogsStructured2()
    {
        // Unstructured
        _logger.LogInformation($"Method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, Query: {HttpContext.Request.QueryString}");
        
        // Structured
        _logger.LogInformation(
            "Method: {RequestMethod}, Path: {RequestPath}, Query: {RequestQueryString}",
            HttpContext.Request.Method,
            HttpContext.Request.Path,
            HttpContext.Request.QueryString);
    }

    [HttpGet("throw/Exception")]
    public void ThrowException()
    {
        throw new Exception("Well that ain't good.");
    }

    [HttpGet("throw/NullReferenceException")]
    public void ThrowNullReferenceException()
    {
        throw new NullReferenceException("Poor little thing... did you forget to null check?");
    }
}
