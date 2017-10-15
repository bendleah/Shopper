using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Models
{
    public class Basket : Entity
    {
        /// <summary>
        /// Dictionary to that holds the order lines for this basket, keyed using the product id. 
        /// </summary>
        public Dictionary<Guid, OrderLine> OrderLines { get; set; }

        public Guid OwnerId { get; set; }
        public DateTime CreatedDate { get; set; }

        public Basket()
        {
            OrderLines = new Dictionary<Guid, OrderLine>();
        }
    }
}
