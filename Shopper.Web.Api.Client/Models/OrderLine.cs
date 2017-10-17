using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Client.Models
{
    public class OrderLine
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
