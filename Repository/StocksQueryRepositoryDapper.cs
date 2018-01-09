using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ManagementStocks.Core.Entities;
using ManagementStocks.Core.Interfaces;
using ManagementStocks.Repository.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockManagement.Utils.QueryUtils;

namespace ManagementStocks.Repository
{
    public class StocksQueryRepositoryDapper : IStocksQueryRepository
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        private const string TableName = "Stocks";


        public StocksQueryRepositoryDapper(IConfiguration config, ILogger<StocksQueryRepositoryDapper> logger)
        {
            _connectionString = config["ConnectionStrings:DefaultConnectionString"];
            _logger = logger;
            _logger.LogInformation("Dapper repository");
        }

        public IReadOnlyList<Stock> Get()
        {
            var sql = $"SELECT * FROM {TableName}";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var result = sqlConnection.Query<Stock>(sql);
                return result.ToList();
            }
        }

        public IReadOnlyList<Stock> Get(QueryParameters queryParameters)
        {
            var sql = $@"select Stocks.*, Products.Name, Products.Description from Stocks
                            join Products on Products.Id = Stocks.ProductId
                            {queryParameters.GetWhereClause()}
                            {queryParameters.GetOrderByClause()}
                            {queryParameters.GetPaginationClause()}";
            var sqlParameters = queryParameters.GetParameters();
            _logger.LogInformation($"Executing {sql} using {JsonConvert.SerializeObject(sqlParameters)}");
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var result = sqlConnection.Query<StockDto>(sql, sqlParameters);
                return result.ToList().Select(x => new Stock
                {
                    Id = x.Id,
                    Product = new Product
                    {
                        Id = x.ProductId,
                        Name = x.Name,
                        Description = x.Description
                    },
                    ProductId = x.ProductId,
                    IsCredit = x.IsCredit,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    OperationTime = x.OperationTime
                }).ToList();
            }

        }

        public Stock Get(Guid id)
        {
            var sql = $@"select Stocks.*, Products.Name, Products.Description from Stocks
                            join Products on Products.Id = Stocks.ProductId
                            WHERE Stocks.Id=@Id";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var result = sqlConnection.QueryFirst<StockDto>(sql, new {Id = id});
                return new Stock
                {
                    Id = result.Id,
                    Product = new Product
                    {
                        Id = result.ProductId,
                        Name = result.Name,
                        Description = result.Description
                    },
                    ProductId = result.ProductId,
                    IsCredit = result.IsCredit,
                    Price = result.Price,
                    Quantity = result.Quantity,
                    OperationTime = result.OperationTime
                };
            }
        }


        public double GetProductQtty(Guid productId)
        {
            var sql = $@"select
                            (select isnull(Sum(Quantity), 0) from Stocks
                            WHERE ProductId=@ProductId
	                            and isnull(IsCredit,0) <> 0) - (
                            select isnull(Sum(Quantity), 0) from Stocks
                            WHERE ProductId=@ProductId
	                            and isnull(IsCredit,0) = 0) as Qtty";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                var result = sqlConnection.QuerySingle(sql, new { ProductId = productId });
                return (double)result.Qtty;
            }
        }
    }
}
