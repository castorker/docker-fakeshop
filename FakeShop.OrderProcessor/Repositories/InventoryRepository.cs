using Dapper;
using System.Data;

namespace FakeShop.OrderProcessor.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IDbConnection _dbConnection;

        public InventoryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? 
                throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<int> GetInventoryForProduct(int productId)
        {
            return await _dbConnection.ExecuteScalarAsync<int>(
                "SELECT QuantityOnHand FROM dbo.Inventory WHERE ProductId = @ProductId",
                new { productId });
        }

        public async Task UpdateInventoryForProduct(int productId, int newInventory)
        {
            await _dbConnection.ExecuteAsync(
                "UPDATE dbo.Inventory SET QuantityOnHand = @newInventory WHERE ProductId = @productId",
                new { productId, newInventory });
        }
    }
}
