using Microsoft.EntityFrameworkCore;
using MyAPI.data;
using MyAPI.endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings_DeaultConnection");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITableService, TableService>();


var app = builder.Build();

if (true)
{
    using var serviceScope = app.Services.CreateScope();
    using var dbContext = serviceScope.ServiceProvider.GetRequiredService<MyDbContext>();
    dbContext.Database.EnsureCreated();
}


app.MapGet("/", () => "Hallo hallo, det er han Addexio!");
app.MapGet("/tables", async (ITableService tableService, CancellationToken cancellationToken) => await tableService.GetTableIdsAsync(cancellationToken));
app.MapGet("/table/{id}", async (long id, ITableService tableService, CancellationToken cancellationToken) => await tableService.GetTableByIdAsync(id, cancellationToken));
app.MapPost("/table", async (ITableService tableService, CancellationToken cancellationToken) => await tableService.CreateTableAsync(cancellationToken));

app.Run();
