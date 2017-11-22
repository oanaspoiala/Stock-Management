using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Core.Entities;
using Dapper;
using ManagementStocks.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockManagement.Utils.QueryUtils;

namespace ManagementStocks.Repository
{
    public class ProductsQueryRepositoryDapper : IQueryRepository<Product>
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        private const string TableName = "Products";

        public ProductsQueryRepositoryDapper(IConfiguration config, ILogger<ProductsQueryRepositoryDapper> logger)
        {
            _connectionString = config["ConnectionStrings:DefaultConnectionString"];
            _logger = logger;
        }

        public IReadOnlyList<Product> Get()
        {
            var sql = $"SELECT * FROM {TableName}";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var result = sqlConnection.Query<Product>(sql);
                return result.ToList();
            }
        }

        public IReadOnlyList<Product> Get(QueryParameters queryParameters)
        {
            var sql = $@"SELECT Products.Id, Products.Name, Products.Description,
                            ISNULL(SUM(Stocks.Quantity), 0) as Qtty FROM Products
                            Left JOIN Stocks on Products.Id = Stocks.ProductId
                            {queryParameters.GetWhereClause()}
                            GROUP By Products.Id, Products.Name, Products.Description
                            {queryParameters.GetOrderByClause()}
                            {queryParameters.GetPaginationClause()}";
            var sqlParameters = queryParameters.GetParameters();
            _logger.LogInformation($"Executing {sql} using {JsonConvert.SerializeObject(sqlParameters)}");
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var result = sqlConnection.Query<Product>(sql, sqlParameters);
                return result.ToList();
            }
        }

        public Product Get(Guid id)
        {
            var sql = $@"SELECT * FROM {TableName}
                            WHERE Id=@Id";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var result = sqlConnection.QueryFirst<Product>(sql, new {Id = id});
                return result;
            }
        }
    }
}
