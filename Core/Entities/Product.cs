using System;
using System.Collections.Generic;
using ManagementStocks.Core.Entities;

namespace Core.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
