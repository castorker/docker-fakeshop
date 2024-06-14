using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.Repositories
{
    public interface IFakeShopRepository
    {
        Task<List<Product>> GetProducts(string category);
        Task SubmitNewOrder(Order order, int customerId, Guid orderId);
    }
}
