using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Tenants;

internal record SetTenantPreferenceCommand(
    TenantId TenantId,
    string Key,
    string? Value,
    string? LastModifiedBy)
    : IRequest;

internal class SetTenantPreferenceCommandHandler : IRequestHandler<SetTenantPreferenceCommand>
{
    private readonly ITenantRepository _tenantRepository;

    public SetTenantPreferenceCommandHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task Handle(SetTenantPreferenceCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenant = await _tenantRepository.GetAsync(request.TenantId, cancellationToken);

        tenant.SetPreference(request.Key, request.Value, request.LastModifiedBy);

        await _tenantRepository.UpdateAsync(tenant, cancellationToken);
    }
}
