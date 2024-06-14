using FakeShop.App.Models;

namespace FakeShop.App.Integrations
{
    public class FakeShopApiClient : IFakeShopApiClient
    {
        private readonly HttpClient _httpClient;

        public FakeShopApiClient(HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = 
                new Uri(configuration.GetValue<string>("FakeShopApiUrl"));
        }

        public async Task<List<Product>> GetProducts(string category = null)
        {
            var requestUri = "products";

            if (!string.IsNullOrWhiteSpace(category))
            {
                requestUri += $"?category={category}";
            }

            var products = await _httpClient
                .GetFromJsonAsync<List<Product>>(requestUri);

            return products;
        }


        //public List<Product> GetProducts(string category = null)
        //{
        //    var products = new List<Product>
        //    {
        //        new Product
        //        {
        //            Id = 1,
        //            Name = "Mountaineering",
        //            Category = "boots",
        //            Price = 200.00,
        //            Description = "Perfect for rugged terrain adventures and alpine trekking."
        //        },
        //        new Product
        //        {
        //            Id = 2,
        //            Name = "Leather",
        //            Category = "boots",
        //            Price = 180.00,
        //            Description = "Hiking boots designed to go further."
        //        },
        //        new Product
        //        {
        //            Id = 3,
        //            Name = "FutureLight",
        //            Category = "boots",
        //            Price = 165.00,
        //            Description = "Optimized for hiking over uneven terrain and slippery rocks."
        //        },
        //        new Product
        //        {
        //            Id = 4,
        //            Name = "Waterproof",
        //            Category = "boots",
        //            Price = 140.00,
        //            Description = "They look like regular hiking boots, but the updated Chilkat Vs takes advantage of a waterproof and breathable construction to keep you dry."
        //        },
        //    };

        //    return products;
        //}

        public async Task<Guid> PlaceOrder(Order order)
        {
            var response = await _httpClient
                .PostAsJsonAsync("orders", order);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Guid>();
            }

            var ex = new Exception("Failed placing Order.");
            ex.Data.Add("Product", order.ProductId);
            ex.Data.Add("Quantity", order.Quantity);
            ex.Data.Add("URI", response.RequestMessage?.RequestUri);
            ex.Data.Add("ResponseCode", (int)response.StatusCode);
            throw ex;
        }

        //public Guid PlaceOrder(Order order)
        //{
        //    return Guid.NewGuid();
        //}
    }
}
