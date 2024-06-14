namespace FakeShop.OrderProcessor.Repositories
{
    public interface IInventoryRepository
    {
        Task<int> GetInventoryForProduct(int productId);
        Task UpdateInventoryForProduct(int productId, int newInventory);
    }
}
