using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using ManagementStocks.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace ManagementStocks.Repository
{
    public class StocksRepository : IStocksRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public StocksRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void Create(Stock stock)
        {
            _databaseContext.Stocks.Add(stock);
            _databaseContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            _databaseContext.Stocks.Remove(Get(id));
            _databaseContext.SaveChanges();
        }

        public IReadOnlyList<Stock> Get()
        {
            return _databaseContext.Stocks.Include(x => x.Product).ToList();
        }

        public Stock Get(Guid id)
        {
            return _databaseContext.Stocks.Include(x => x.Product).FirstOrDefault(x => x.Id == id);
        }

        public void Update(Stock stock)
        {
            _databaseContext.Stocks.Update(stock);
            _databaseContext.SaveChanges();
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
