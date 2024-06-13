// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Serilog;

Console.Title = "Serilog";

//http://bit.ly/default-builder-source
IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
    .AddJsonFile("appsettings.json", false)
    .AddEnvironmentVariables()
    .Build();

ConfigureLogging();

try
{
    var connectionString = config.GetConnectionString("Database");
    var simpleProperty = config.GetValue<string>("SimpleProperty");
    var nestedProp = config.GetValue<string>("ComplexProperty:NestedProperty");

    Log.ForContext("ConnectionString", connectionString)
       .ForContext("SimpleProperty", simpleProperty)
       .ForContext("ComplexProperty:NestedProperty", nestedProp)
       .Information("Loaded configuration!", connectionString);


    Log.ForContext("Args", args)
       .Information("Starting program...");

    Console.WriteLine("Hello Docker World!"); // do some invoice generation

    Log.Information("Finished execution!");
}
catch (Exception ex)
{
    Log.Error(ex, "Some kind of exception occurred.");
}
finally
{
    Log.CloseAndFlush();
}

static void ConfigureLogging()
{
    var name = typeof(Program).Assembly.GetName().Name;

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithProperty("Assembly", name)
        .WriteTo.Console()
        .WriteTo.Seq("http://host.docker.internal:5341")
        .CreateLogger();
}
