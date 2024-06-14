using Microsoft.AspNetCore.Mvc;

namespace AntuDevOps.PointOfSale.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController
{
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
