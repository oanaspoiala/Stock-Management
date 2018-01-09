using ManagementStocks.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    public sealed class DatabaseContext :DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            :base(options)
        {
            // Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}
