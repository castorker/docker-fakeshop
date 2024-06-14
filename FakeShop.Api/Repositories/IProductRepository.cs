using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsForCategory(string category);
    }
}
