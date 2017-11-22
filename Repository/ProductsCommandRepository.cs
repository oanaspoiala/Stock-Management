using System;
using Core.Entities;
using ManagementStocks.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Persistance;

namespace ManagementStocks.Repository
{
    public class ProductsCommandRepository : ICommandRepository<Product>
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly ILogger _logger;

        public ProductsCommandRepository(IDatabaseContext databaseContext, ILogger<ProductsCommandRepository> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public void Create(Product product)
        {
            _databaseContext.Products.Add(product);
            _databaseContext.SaveChanges();
        }

        public void Update(Product product)
        {
            _databaseContext.Products.Update(product);
            _databaseContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var product = _databaseContext.Products.Find(id);
            if (product != null)
            {
                _databaseContext.Products.Remove(product);
                _databaseContext.SaveChanges();
            }
        }
    }
}
