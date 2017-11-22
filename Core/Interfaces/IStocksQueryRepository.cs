using System;
using ManagementStocks.Core.Entities;

namespace ManagementStocks.Core.Interfaces
{
    public interface IStocksQueryRepository : IQueryRepository<Stock>
    {
        double GetProductQtty(Guid productId);
    }
}
