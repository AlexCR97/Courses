using AntuDevOps.PointOfSale.Api.OAuth;
using AntuDevOps.PointOfSale.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AntuDevOps.PointOfSale.Api.Controllers;

[ApiController]
[Route("api/connect")]
public class ConnectController : ControllerBase
{
    private readonly ISender _sender;
    private readonly JwtOptions _jwtOptions;
    private readonly JwtService _jwtService;

    public ConnectController(ISender sender, JwtOptions jwtOptions, JwtService jwtService)
    {
        _sender = sender;
        _jwtOptions = jwtOptions;
        _jwtService = jwtService;
    }

    [HttpPost("token")]
    public async Task<OAuthTokenResponse> ExchangeToken([FromForm] IFormCollection form)
    {
        var tenantId = int.TryParse(form["tenantId"], out var outTenantId)
            ? outTenantId
            : 0;

        var email = form["email"].ToString();

        var password = form["password"].ToString();

        var loginResult = await _sender.Send(new LoginRequest(
            new Domain.Models.TenantId(tenantId),
            new Domain.Models.Email(email),
            password));

        var accessToken = _jwtService.GenerateJwt(new List<Claim>
        {
            new Claim("tenantId", loginResult.Tenant.TenantId.Value.ToString(), ClaimValueTypes.Integer32),
            new Claim("userId", loginResult.User.Id.ToString(), ClaimValueTypes.Integer32),
            new Claim("email", loginResult.User.Email.OriginalValue),
            new Claim("role", loginResult.Tenant.Role.ToString()),
        });

        return new OAuthTokenResponse(
            accessToken,
            "Bearer",
            _jwtOptions.ExpiresInMinutes,
            null,
            null,
            null);
    }
}
