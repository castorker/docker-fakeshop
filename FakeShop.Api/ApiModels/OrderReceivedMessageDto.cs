using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.ApiModels
{
    public class OrderReceivedMessageDto
    {
        public Order Order { get; set; }
        public int CustomerId { get; set; }
        public Guid OrderId { get; set; }
    }
}
