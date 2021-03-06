﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Dto
{
    public class OrderLineDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Quantity * Price;
            }
        }
    }
}
