using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementStocks.Repository.DTO
{
    public class StockDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime OperationTime { get; set; }

        public Guid ProductId { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        public bool IsCredit { get; set; }
    }
}
