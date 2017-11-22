using System;
using Core.Entities;

namespace ManagementStocks.MVC.Models
{
    public class StockViewModel
    {
        public Guid Id { get; set; }
        public DateTime OperationTime { get; set; }
        public Product Product { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public bool IsCredit { get; set; }
    }
}
