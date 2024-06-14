using FakeShop.OrderProcessor.Models;
using FakeShop.OrderProcessor.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text.Json;

namespace FakeShop.OrderProcessor
{
    public class Worker : BackgroundService
    {
        private readonly IInventoryRepository _repository;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private string queueName = "order.received";
        private EventingBasicConsumer _consumer;

        public Worker(IConfiguration configuration,
            IInventoryRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMqHost")
            };

            try
            {
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (_connection != null)
                        continue;
                    Thread.Sleep(3000);
                    try { _connection = factory.CreateConnection(); } 
                    catch { }
                }
                if (_connection == null) throw;
            }

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName, 
                durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += ProcessOrderReceived;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _channel.BasicConsume(queue: queueName, autoAck: true, consumer: _consumer);
            }

            return Task.CompletedTask;
        }

        private async void ProcessOrderReceived(object? sender, BasicDeliverEventArgs eventArgs)
        {
            var orderInfo = JsonSerializer.Deserialize<OrderReceivedMessage>(eventArgs.Body.ToArray());

            Log.ForContext("OrderReceived", orderInfo, true)
                .Information("Received message from queue for processing.");

            var currentQuantity = await _repository
                .GetInventoryForProduct(orderInfo.Order.ProductId);

            await _repository.UpdateInventoryForProduct(orderInfo.Order.ProductId,
                currentQuantity - orderInfo.Order.Quantity);
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        Log.Information("Worker running at: {time}", DateTimeOffset.UtcNow);
        //        await Task.Delay(1000, stoppingToken);
        //    }
        //}
    }

    //public class Worker : BackgroundService
    //{
    //    private readonly ILogger<Worker> _logger;

    //    public Worker(ILogger<Worker> logger)
    //    {
    //        _logger = logger;
    //    }

    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            if (_logger.IsEnabled(LogLevel.Information))
    //            {
    //                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
    //            }
    //            await Task.Delay(1000, stoppingToken);
    //        }
    //    }
    //}
}
