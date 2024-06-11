using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Minitwit.Web.Logging;
using Prometheus;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.Http;

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
builder.Host.UseSerilog();

var app = builder.Build();

var log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200")) // replace with your Elasticsearch URL
    {
        AutoRegisterTemplate = true,
    })
    .CreateLogger();

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