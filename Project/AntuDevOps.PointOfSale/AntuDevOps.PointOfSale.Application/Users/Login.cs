using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Users;

public record LoginRequest(
    TenantId TenantId,
    Email Email,
    string Password)
    : IRequest<LoginResult>;

public record LoginResult(UserTenant Tenant, User User);

internal class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Password)
            .NotNull();
    }
}

internal class LoginRequestHandler : IRequestHandler<LoginRequest, LoginResult>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;

    public LoginRequestHandler(ITenantRepository tenantRepository, IUserRepository userRepository)
    {
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
    }

    public async Task<LoginResult> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        new LoginRequestValidator().ValidateAndThrow(request);

        var tenant = await _tenantRepository.GetAsync(request.TenantId, cancellationToken);

        var user = await _userRepository.GetByEmailOrDefaultAsync(request.Email, cancellationToken)
            ?? throw new InvalidCredentialsException();

        var userTenant = user.GetTenantOrDefault(tenant.Id)
            ?? throw new InvalidCredentialsException();

        if (user.Password.Value != request.Password)
            throw new InvalidCredentialsException();

        return new LoginResult(userTenant, user);
    }
}

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base("Invalid credentials")
    {
    }
}
