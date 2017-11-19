using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Stock
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        public bool IsCredit { get; set; }
    }
}
