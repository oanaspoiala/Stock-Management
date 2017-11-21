using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Entities;
using ManagementStocks.Core.Interfaces;
using Persistance;

namespace ManagementStocks.Repository
{
    public class ProductsQueryRepository : IQueryRepository<Product>
    {
        private readonly IDatabaseContext _databaseContext;

        public ProductsQueryRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IReadOnlyList<Product> Get()
        {
           return _databaseContext.Products.ToList();
        }

        public Product Get(Guid id)
        {
           return _databaseContext.Products.Find(id);
        }
    }
}
