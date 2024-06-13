using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly List<string> _validCategories = new List<string> { "all", "boots", "waterproof jackets", "sleeping bags", "polars" };
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ILogger<ProductRepository> logger)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Product> GetProductsForCategory(string category)
        {
            _logger.LogInformation("Starting logic to get products for {category}", category);

            if (!_validCategories.Any(c =>
                string.Equals(category, c, StringComparison.InvariantCultureIgnoreCase)))
            {
                // invalid category -- bad request
                throw new ApplicationException($"Unrecognized category: {category}.  " +
                         $"Valid categories are: [{string.Join(",", _validCategories)}]");
            }

            if (string.Equals(category, "polars", StringComparison.InvariantCultureIgnoreCase))
            {
                // simulate database error or real technical error like not implemented exception
                throw new Exception("Not implemented! No polars have been defined in 'database' yet!!!!");
            }

            return GetAllProducts().Where(a =>
                string.Equals("all", category, StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(category, a.Category, StringComparison.InvariantCultureIgnoreCase));

        }

        private static IReadOnlyList<Product> GetAllProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Mountaineering", Category = "boots", Price = 200.00,
                    Description = "Perfect for rugged terrain adventures and alpine trekking." },
                new Product { Id = 2, Name = "Leather", Category = "boots", Price = 180.00,
                    Description = "Hiking boots designed to go further." },
                new Product { Id = 3, Name = "FutureLight", Category = "boots", Price = 165.00,
                    Description = "Optimized for hiking over uneven terrain and slippery rocks."},
                new Product { Id = 4, Name = "Waterproof", Category = "boots", Price = 140.00,
                    Description = "They look like regular hiking boots, but the updated Chilkat Vs takes advantage of a waterproof and breathable construction to keep you dry." },
            };
        }
    }
}
