using Dapper;
using FakeShop.Api.Domain.Entities;
using System.Data;

namespace FakeShop.Api.Repositories
{
    public class FakeShopRepository : IFakeShopRepository
    {
        private readonly IDbConnection _dbConnection;

        public FakeShopRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? 
                throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<Product>> GetProducts(string category)
        {
            return (await _dbConnection.QueryAsync<Product>(
                "SELECT * FROM dbo.Product WHERE Category = COALESCE(@category, Category) OR @category = 'all' ",
                new { category })).ToList();
        }

        public async Task SubmitNewOrder(Order order, int customerId, Guid orderId)
        {
            await _dbConnection.ExecuteScalarAsync<int>(
                "INSERT INTO dbo.[Order] (OrderId, CustomerId, ProductId, QuantityOrdered, OrderTotal) VALUES " +
                                          "(@OrderId, @CustomerId, @ProductId, @Quantity, @Total)", new
                                          {
                                              orderId,
                                              customerId,
                                              order.ProductId,
                                              order.Quantity,
                                              Total = 1.00  // TODO: retrieve price and multiply by quantity
                                          });
        }
    }
}
