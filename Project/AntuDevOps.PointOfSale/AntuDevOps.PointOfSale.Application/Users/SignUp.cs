using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Users;

public record SignUpCommand(
    string TenantName,
    UserTenantRole Role,
    Email Email,
    Password Password,
    string FirstName,
    string LastName)
    : IRequest<UserSignedUpResult>;

public record UserSignedUpResult(
    TenantId TenantId,
    UserId UserId);

internal class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.TenantName)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}

internal class SignUpCommandHandler : IRequestHandler<SignUpCommand, UserSignedUpResult>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;

    public SignUpCommandHandler(ITenantRepository tenantRepository, IUserRepository userRepository)
    {
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
    }

    public async Task<UserSignedUpResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        new SignUpCommandValidator().ValidateAndThrow(request);

        var tenant = await _tenantRepository.GetByNameOrDefaultAsync(request.TenantName);

        if (tenant is null)
        {
            tenant = Tenant.SignUp(request.TenantName);
            await _tenantRepository.CreateAsync(tenant, cancellationToken);
        }

        var user = User.SignUp(
            tenant,
            request.Role,
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        var userId = await _userRepository.CreateAsync(user);

        return new UserSignedUpResult(
            tenant.Id,
            userId);
    }
}
