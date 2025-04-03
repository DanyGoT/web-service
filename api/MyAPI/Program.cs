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

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (OperationCanceledException)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Operation canceled");
    }
});


app.MapGet("/", () 
    => "Hallo hallo, det er han Addexio!");
app.MapGet("/tables", async (ITableService tableService, CancellationToken cancellationToken) 
    => await tableService.GetTableIdsAsync(cancellationToken));
app.MapGet("/table/{id}", async (long id, ITableService tableService, CancellationToken cancellationToken) 
    => await tableService.GetTableByIdAsync(id, cancellationToken));
app.MapPost("/table", async (ITableService tableService, CancellationToken cancellationToken) 
    => await tableService.CreateTableAsync(cancellationToken));
app.MapPut("/table/{tableId}/{title}", async (long tableId, string title, ITableService tableService, CancellationToken cancellationToken)
    => await tableService.UpdateTableTitleAsync(tableId, title, cancellationToken));

app.MapPost("/table/{id}/items", async(long id, ITableService tableService, CancellationToken cancellationToken)
    => await tableService.CreateTableItemAsync(id, cancellationToken));
app.MapPut("/table/items/{tableItemId}", async(long tableItemId, string value, ITableService tableService, CancellationToken cancellationToken)
    => await tableService.UpdateTableItemAsync(tableItemId, value, cancellationToken));
app.MapDelete("/table/items/{tableItemId}", async (long tableItemId, ITableService tableService, CancellationToken cancellationToken)
    => await tableService.DeleteTableItemAsync(tableItemId, cancellationToken));

app.Run();
