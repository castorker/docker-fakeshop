using FakeShop.Api.ApiModels;
using FakeShop.Api.Domain.Entities;
using RabbitMQ.Client;
using Serilog;
using System.Text.Json;

namespace FakeShop.Api.Integrations
{
    public class OrderProcessingNotification : IOrderProcessingNotification
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private string queueName = "order.received";

        public OrderProcessingNotification(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMqHost")
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName, 
                durable: false, exclusive: false, autoDelete: false,
                arguments: null);
        }
        public void OrderReceived(Order order, int customerId, Guid orderId)
        {
            var message = new OrderReceivedMessageDto 
            { Order = order, CustomerId = customerId, OrderId = orderId };

            var messageBytes = JsonSerializer.SerializeToUtf8Bytes(message);
            _channel.BasicPublish("", routingKey: queueName, basicProperties: null, body: messageBytes);
            Log.ForContext("Body", message, true)
               .Information("Published order notification");
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
