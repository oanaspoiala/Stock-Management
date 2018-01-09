using System;
using System.Collections.Generic;
using System.Linq;
using ManagementStocks.Core.Entities;
using ManagementStocks.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Persistance;
using StockManagement.Utils.QueryUtils;

namespace ManagementStocks.Repository
{
    public class ProductsQueryRepository : IQueryRepository<Product>
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly ILogger _logger;

        public ProductsQueryRepository(IDatabaseContext databaseContext, ILogger<ProductsQueryRepository> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
            _logger.LogInformation("Entity framework repository");
        }

        public IReadOnlyList<Product> Get()
        {
            return _databaseContext.Products.ToList();
        }

        public IReadOnlyList<Product> Get(QueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public Product Get(Guid id)
        {
            return _databaseContext.Products.Find(id);
        }
    }
}
