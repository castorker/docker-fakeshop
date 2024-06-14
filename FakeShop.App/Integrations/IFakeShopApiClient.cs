using FakeShop.App.Models;

namespace FakeShop.App.Integrations
{
    public interface IFakeShopApiClient
    {
        Task<List<Product>> GetProducts(string category = null);
        Task<Guid> PlaceOrder(Order order);

        //List<Product> GetProducts(string category = null);
        //Guid PlaceOrder(Order order);
    }
}
