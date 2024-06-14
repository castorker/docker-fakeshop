using FakeShop.OrderProcessor;
using Serilog.Events;
using Serilog;
using FakeShop.OrderProcessor.Repositories;
using System.Data.SqlClient;
using System.Data;

var name = typeof(Program).Assembly.GetName().Name;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Assembly", name)
    //.WriteTo.Seq("http://host.docker.internal:5341")
    .WriteTo.Seq("http://seq_in_dc:5341")
    .WriteTo.Console()
    .CreateLogger();

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<IDbConnection>(
            db => new SqlConnection(hostContext.Configuration
            .GetConnectionString("Database")));
        services.AddSingleton<IInventoryRepository, InventoryRepository>();
        services.AddHostedService<Worker>();
    })
    .UseSerilog();

var host = builder.Build();

host.Run();


//var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

//var host = builder.Build();
//host.Run();
