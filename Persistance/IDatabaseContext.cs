using ManagementStocks.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    public interface IDatabaseContext
    {
        int SaveChanges();
        DbSet<Product> Products { get; set; }
        DbSet<Stock> Stocks { get; set; }
    }
}
