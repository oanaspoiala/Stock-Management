using System;

namespace ManagementStocks.MVC.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Qtty { get; set; }
    }
}
