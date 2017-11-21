using System;
using System.Collections.Generic;
using Core.Entities;

namespace ManagementStocks.Core.Interfaces
{
    public interface IStocksQueryRepository : IQueryRepository<Stock>
    {
        double GetProductQtty(Guid productId);
    }
}
