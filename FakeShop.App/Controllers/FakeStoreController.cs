using FakeShop.App.Integrations;
using FakeShop.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace FakeShop.App.Controllers
{
    public class FakeStoreController : Controller
    {
        private readonly IFakeShopApiClient _apiClient;

        public FakeStoreController(IFakeShopApiClient apiClient)
        {
            _apiClient = apiClient ?? 
                throw new ArgumentNullException(nameof(apiClient));
        }

        public async Task<IActionResult> Index()
        {
            var products = await _apiClient.GetProducts();
            return View(products);
        }

        public async Task<IActionResult> Order(int id) 
        {
            var orderId = await _apiClient.PlaceOrder(
                new Order 
                { 
                    ProductId = id, 
                    Quantity = 1 
                });

            return View("OrderConfirmation", orderId);
        }
    }
}
