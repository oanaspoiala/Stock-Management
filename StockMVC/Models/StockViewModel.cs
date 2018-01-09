using System;
using System.ComponentModel.DataAnnotations;
using ManagementStocks.Core.Entities;

namespace ManagementStocks.MVC.Models
{
    public class StockViewModel
    {
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Operation time")]
        public DateTime OperationTime { get; set; }

        public Product Product { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        [Display(Name = "Is credit")]
        public bool IsCredit { get; set; }
    }
}
