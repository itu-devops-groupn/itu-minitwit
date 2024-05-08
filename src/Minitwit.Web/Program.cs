using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Minitwit.Web.Logging;
using Prometheus;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connString = File.ReadAllText("/data/connstring.txt");
builder.Services.AddDbContext<MinitwitContext>(options =>
        options.UseNpgsql(connString));
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.UseHttpClientMetrics();

// Add Serilog
ConfigureLogging();
builder.Host.UseSerilog();

var app = builder.Build();

void ConfigureLogging()
{
    string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{environment}.json", optional: true
        ).Build();
        
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions (new Uri(configuration["ElasticConfiguration:Uri"])) {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
        NumberOfReplicas = 1,
        NumberOfShards = 2
    };
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var MinitwitContext = scope.ServiceProvider.GetRequiredService<MinitwitContext>();
        _ = MinitwitContext.Database.EnsureCreated();
    }
    _ = app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseHttpMetrics();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapRazorPages(); // Map Razor Pages
            _ = endpoints.MapMetrics(); // Map Metrics
        });

app.Run();