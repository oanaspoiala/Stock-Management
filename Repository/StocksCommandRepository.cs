using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using ManagementStocks.Core.Interfaces;
using Persistance;

namespace ManagementStocks.Repository
{
    public class StocksCommandRepository : ICommandRepository<Stock>
    {
        private readonly IDatabaseContext _databaseContext;

        public StocksCommandRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void Create(Stock stock)
        {
            _databaseContext.Stocks.Add(stock);
            _databaseContext.SaveChanges();
        }

        public void Update(Stock stock)
        {
            _databaseContext.Stocks.Update(stock);
            _databaseContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var stock = _databaseContext.Stocks.Find(id);
            if (stock == null)
            {
                return;
            }
            _databaseContext.Stocks.Remove(stock);
            _databaseContext.SaveChanges();
        }
    }
}
