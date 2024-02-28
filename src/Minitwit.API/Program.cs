using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MinitwitContext>(options => 
        options.UseSqlite($"Data Source=/tmp/test-minitwit.db"));
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Init DB
File.Delete("/tmp/test-minitwit.db");
File.Create("/tmp/test-minitwit.db").Close();
string script = File.ReadAllText("/src/schema.sql");

using (SqlConnection connection = new SqlConnection("Data Source=/tmp/test-minitwit.db"))
{
    connection.Open();
    SqlCommand command = new SqlCommand(script, connection);
    command.ExecuteNonQuery();
}



app.Run();
