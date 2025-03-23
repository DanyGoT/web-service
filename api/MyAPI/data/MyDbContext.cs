using Microsoft.EntityFrameworkCore;
using MyAPI.data.Items;

namespace MyAPI.data;
public class MyDbContext : DbContext
{
    public DbSet<MyTable> MyTables { get; set; }
    public DbSet<MyTableItem> MyTableItems { get; set; }
    public MyDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MyTable>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<MyTableItem>()
            .HasKey(t => t.Id);
    }
}

