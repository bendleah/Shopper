using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Models
{
    public class OrderLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
