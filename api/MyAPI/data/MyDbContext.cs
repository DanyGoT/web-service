using Microsoft.EntityFrameworkCore;

namespace MyAPI.data;
public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions options) : base(options)
    {
    }
}

