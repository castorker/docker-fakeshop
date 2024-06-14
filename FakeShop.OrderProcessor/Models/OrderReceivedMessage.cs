namespace FakeShop.OrderProcessor.Models
{
    public class OrderReceivedMessage
    {
        public Order Order { get; set; }
        public int CustomerId { get; set; }
        public Guid OrderId { get; set; }
    }
}
