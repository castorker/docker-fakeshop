using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsForCategory(string category);
    }
}
