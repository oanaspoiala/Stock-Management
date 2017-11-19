using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Entities;
using Persistance;

namespace Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public ProductsRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void Create(Product product)
        {
            _databaseContext.Products.Add(product);
            _databaseContext.SaveChanges();
        }

        public IReadOnlyList<Product> Get()
        {
           return _databaseContext.Products.ToList();
        }

        public Product Get(Guid id)
        {
           return _databaseContext.Products.Find(id);
        }

        public void Update(Product product)
        {
            _databaseContext.Products.Update(product);
            _databaseContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            _databaseContext.Products.Remove(Get(id));
            _databaseContext.SaveChanges();
        }
    }
}
