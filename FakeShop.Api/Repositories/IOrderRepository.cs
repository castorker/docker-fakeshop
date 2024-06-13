using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Guid PlaceOrder(Order order, int customerId);
    }
}
