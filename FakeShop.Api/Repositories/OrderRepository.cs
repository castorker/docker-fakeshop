using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ILogger<OrderRepository> logger)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
        }

        public Guid PlaceOrder(Order order, int customerId)
        {
            _logger.LogInformation("Placing order and sending update for inventory...");
            
            // persist order to database or elsewhere

            // post "orderplaced" event to rabbitmq

            return Guid.NewGuid();
        }
    }
}
