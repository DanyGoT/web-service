using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyAPI.data;
using MyAPI.data.Items;

namespace MyAPI.endpoints;


public interface ITableService
{
    Task<List<long>> GetTableIdsAsync(CancellationToken cancellationToken);
    Task<MyTable?> GetTableByIdAsync(long id, CancellationToken cancellationToken);
    Task CreateTableAsync(CancellationToken cancellationToken);
}

public class TableService : ITableService
{
    private readonly MyDbContext _context;

    public TableService(MyDbContext context)
    {
        _context = context;
    }


    public async Task<List<long>> GetTableIdsAsync(CancellationToken cancellationToken) => await _context.MyTables.Select(t => t.Id).ToListAsync(cancellationToken);

    public async Task<MyTable?> GetTableByIdAsync(long id, CancellationToken cancellationToken)
        => await _context.MyTables.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task CreateTableAsync(CancellationToken cancellationToken) {
        await _context.MyTables.AddAsync(new MyTable(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}