using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connString;
if (builder.Environment.IsDevelopment())
{
    connString = "Data Source=/tmp/minitwit.db";
}
else
{
    connString = "Data Source=/data/minitwit.db";
}
builder.Services.AddDbContext<MinitwitContext>(options => 
        options.UseSqlite(connString));
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

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

app.UseAuthorization();

app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapRazorPages(); // Map Razor Pages
            _ = endpoints.MapControllers(); // Map controllers
        });

app.Run();