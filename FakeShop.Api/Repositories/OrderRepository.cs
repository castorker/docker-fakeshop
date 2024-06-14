using FakeShop.Api.Domain.Entities;
using FakeShop.Api.Integrations;

namespace FakeShop.Api.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ILogger<OrderRepository> _logger;
        private readonly IOrderProcessingNotification _orderProcessingNotification;

        public OrderRepository(ILogger<OrderRepository> logger,
            IOrderProcessingNotification orderProcessingNotification)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _orderProcessingNotification = orderProcessingNotification ?? 
                throw new ArgumentNullException(nameof(orderProcessingNotification));
        }

        public Guid PlaceOrder(Order order, int customerId)
        {
            _logger.LogInformation("Placing order and sending update for inventory...");
            var orderId = Guid.NewGuid();

            // persist order to database or elsewhere

            // post "orderplaced" event to rabbitmq
            _orderProcessingNotification.OrderReceived(order, customerId, orderId);

            return orderId;
        }
    }
}
