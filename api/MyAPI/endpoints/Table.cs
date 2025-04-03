using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyAPI.data;
using MyAPI.data.Items;
using System.Collections.Immutable;

namespace MyAPI.endpoints;


public interface ITableService
{
    Task<List<long>> GetTableIdsAsync(CancellationToken cancellationToken);
    Task<MyTable?> GetTableByIdAsync(long tableId, CancellationToken cancellationToken);
    Task UpdateTableTitleAsync (long tableId, string title, CancellationToken cancellationToken);
    Task CreateTableAsync(CancellationToken cancellationToken);
    Task CreateTableItemAsync(long tableId, CancellationToken cancellationToken);
    Task UpdateTableItemAsync(long tableItemId, string value, CancellationToken cancellationToken);
    Task DeleteTableItemAsync(long tableItemId, CancellationToken cancellationToken);
}

public class TableService : ITableService
{
    private readonly MyDbContext _context;

    public TableService(MyDbContext context)
    {
        _context = context;
    }


    public async Task<List<long>> GetTableIdsAsync(CancellationToken cancellationToken) => await _context.MyTables.Select(t => t.Id).ToListAsync(cancellationToken);

    public async Task<MyTable?> GetTableByIdAsync(long tableId, CancellationToken cancellationToken)
        => await _context.MyTables.Include(t => t.Items).FirstOrDefaultAsync(t => t.Id == tableId, cancellationToken);

    public async Task CreateTableAsync(CancellationToken cancellationToken) {
        await _context.MyTables.AddAsync(new MyTable(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTableTitleAsync(long tableId, string title, CancellationToken cancellationToken) {
        var table = await _context.MyTables.FirstOrDefaultAsync(t => t.Id == tableId, cancellationToken)
            ?? throw new InvalidOperationException(message: "Could not alter table. Table not found");
        table.Title = title;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateTableItemAsync(long tableId, CancellationToken cancellationToken) {
        var table = await _context.MyTables.FirstOrDefaultAsync(t => t.Id == tableId, cancellationToken)
            ?? throw new InvalidOperationException(message: "Could not create table-item. Table not found");
        table.Items.Add(new MyTableItem());
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateTableItemAsync(long tableItemId, string value, CancellationToken cancellationToken) {
        var tableItem = await _context.MyTableItems.FirstOrDefaultAsync(ti => ti.Id == tableItemId, cancellationToken) 
            ?? throw new InvalidOperationException(message: "Could not update table-item. Item not found");
        tableItem.Value = value;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTableItemAsync(long tableItemId, CancellationToken cancellationToken) {
        await _context.MyTableItems.Where(ti => ti.Id == tableItemId).ExecuteDeleteAsync(cancellationToken);
    }
}