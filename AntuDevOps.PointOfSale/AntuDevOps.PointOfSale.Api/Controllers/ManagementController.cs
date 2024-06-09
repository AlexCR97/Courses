using AntuDevOps.PointOfSale.Application.Seeding;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AntuDevOps.PointOfSale.Api.Controllers;

[ApiController]
[Route("api/management")]
public class ManagementController
{
    private readonly ISender _sender;

    public ManagementController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("seed")]
    public async Task Seed()
    {
        await _sender.Send(new SeedCommand("Anonymous"));
    }
}
