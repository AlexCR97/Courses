using AntuDevOps.PointOfSale.Domain.Models;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Tenants;

public record SelectWarehouseCommand(
    TenantId TenantId,
    WarehouseId WarehouseId,
    string? LastModifiedBy)
    : IRequest;

internal class SelectWarehouseCommandHandler : IRequestHandler<SelectWarehouseCommand>
{
    private readonly ISender _sender;

    public SelectWarehouseCommandHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task Handle(SelectWarehouseCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _sender.Send(new SetTenantPreferenceCommand(
            request.TenantId,
            TenantPreferenceKeys.SelectedWarehouse,
            request.WarehouseId.Value.ToString(),
            request.LastModifiedBy),
            cancellationToken);
    }
}
