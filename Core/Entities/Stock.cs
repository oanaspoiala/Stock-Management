using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;

namespace ManagementStocks.Core.Entities
{
    public class Stock
    {
        public Stock()
        {
            OperationTime = DateTime.Now;  
        }

        public Guid Id { get; set; }

        public DateTime OperationTime { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        public bool IsCredit { get; set; }
    }
}
