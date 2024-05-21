using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Domain.Repositories;

public interface IOrderRepository : IRepository<Order, OrderId>
{
}
