using FakeShop.Api;
using FakeShop.Api.Middleware;
using FakeShop.Api.Repositories;
using Serilog;
using Serilog.Events;

var name = typeof(Program).Assembly.GetName().Name;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Assembly", name)

    // available sinks: https://github.com/serilog/serilog/wiki/Provided-Sinks
    // Seq: https://datalust.co/seq
    // Seq with Docker: https://docs.datalust.co/docs/getting-started-with-docker

    //.WriteTo.Seq(serverUrl: "http://host.docker.internal:5341")
    .WriteTo.Seq(serverUrl: "http://seq_in_dc:5341")

    .WriteTo.Console()
    .WriteTo.File("logs/FakeShopApi.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("Database");
var simpleProperty = builder.Configuration.GetValue<string>("SimpleProperty");
var nestedProp = builder.Configuration.GetValue<string>("ComplexProperty:NestedProperty");

//Log.ForContext("ConnectionString", connectionString)
//   .ForContext("SimpleProperty", simpleProperty)
//   .ForContext("ComplexProperty:NestedProperty", nestedProp)
//   .Information("Loaded configuration!", connectionString);

var dbgView = (builder.Configuration as IConfigurationRoot).GetDebugView();
Log.ForContext("ConfigurationDebug", dbgView)
    .Information("Configuration dump.");

// Add services to the container.

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<CustomExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
