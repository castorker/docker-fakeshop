using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.Integrations
{
    public interface IOrderProcessingNotification
    {
        void OrderReceived(Order order, int customerId, Guid orderId);
    }
}
