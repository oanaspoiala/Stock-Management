using System;
using Core.Entities;
using ManagementStocks.Core.Interfaces;
using Persistance;

namespace ManagementStocks.Repository
{
    public class ProductsCommandRepository : ICommandRepository<Product>
    {
        private readonly IDatabaseContext _databaseContext;

        public ProductsCommandRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
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
