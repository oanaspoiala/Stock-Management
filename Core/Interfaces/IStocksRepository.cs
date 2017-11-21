using System;
using System.Collections.Generic;
using Core.Entities;

namespace ManagementStocks.Core.Interfaces
{
    public interface IStocksRepository
    {
        void Create(Stock stock);

        IReadOnlyList<Stock> Get();

        Stock Get(Guid id);

        void Update(Stock stock);

        void Delete(Guid id);

        double GetProductQtty(Guid productId);
    }
}
