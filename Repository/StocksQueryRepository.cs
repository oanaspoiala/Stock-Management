using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagementStocks.Core.Entities;
using ManagementStocks.Core.Interfaces;
using Persistance;
using StockManagement.Utils.QueryUtils;

namespace ManagementStocks.Repository
{
    public class StocksQueryRepository : IStocksQueryRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public StocksQueryRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public IReadOnlyList<Stock> Get()
        {
            return _databaseContext.Stocks.ToList();
        }

        public IReadOnlyList<Stock> Get(QueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public Stock Get(Guid id)
        {
            return _databaseContext.Stocks.Find(id);
        }

        public double GetProductQtty(Guid productId)
        {
            var credit = _databaseContext.Stocks.Any(x => x.Product.Id == productId && x.IsCredit)
                ? _databaseContext.Stocks.Where(x => x.Product.Id == productId && x.IsCredit).Sum(x => x.Quantity)
                : 0;

            var debit = _databaseContext.Stocks.Any(x => x.Product.Id == productId && !x.IsCredit)
                ? _databaseContext.Stocks.Where(x => x.Product.Id == productId && !x.IsCredit).Sum(x => x.Quantity)
                : 0;
            return credit - debit;
        }
    }
}
