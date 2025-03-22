using Microsoft.EntityFrameworkCore;
using MyAPI.data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();
