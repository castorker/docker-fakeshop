using FakeShop.Api.Domain.Entities;
using FakeShop.Api.Integrations;

namespace FakeShop.Api.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly IFakeShopRepository _repository;
        private readonly ILogger<OrderRepository> _logger;
        private readonly IOrderProcessingNotification _orderProcessingNotification;

        public OrderRepository(IFakeShopRepository repository, 
            ILogger<OrderRepository> logger,
            IOrderProcessingNotification orderProcessingNotification)
        {
            _repository = repository ?? 
                throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _orderProcessingNotification = orderProcessingNotification ?? 
                throw new ArgumentNullException(nameof(orderProcessingNotification));
        }

        public async Task<Guid> PlaceOrder(Order order, int customerId)
        {
            _logger.LogInformation("Placing order and sending update for inventory...");
            var orderId = Guid.NewGuid();

            // persist order to database or elsewhere
            await _repository.SubmitNewOrder(order, customerId, orderId);

            // post "orderplaced" event to rabbitmq
            _orderProcessingNotification.OrderReceived(order, customerId, orderId);

            return orderId;
        }
    }
}
