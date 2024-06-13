using Serilog;

namespace FakeShop.OrderProcessor
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information("Worker running at: {time}", DateTimeOffset.UtcNow);
                await Task.Delay(1000, stoppingToken);
            }
        }
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
