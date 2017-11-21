using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using ManagementStocks.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance;
using StockManagement.Utils.QueryUtils;

namespace ManagementStocks.Repository
{
    public class StocksQueryRepository : IStocksQueryRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public StocksQueryRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IReadOnlyList<Stock> Get()
        {
            return _databaseContext.Stocks.Include(x => x.Product).ToList();
        }

        public IReadOnlyList<Stock> Get(QueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public Stock Get(Guid id)
        {
            return _databaseContext.Stocks.Include(x => x.Product).FirstOrDefault(x => x.Id == id);
        }

        public double GetProductQtty(Guid productId)
        {
            if (!_databaseContext.Stocks.Any(x => x.ProductId == productId))
            {
                return 0;
            }
            return _databaseContext.Stocks.Where(x => x.ProductId == productId && x.IsCredit).Sum(x => x.Quantity) -
                   _databaseContext.Stocks.Where(x => x.ProductId == productId && !x.IsCredit).Sum(x => x.Quantity);
        }
    }
}
