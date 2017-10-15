using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Dto
{
    public class OrderLineUpdateDto
    { 
        [Range(0, 99)]
        public int Quantity { get; set; }
    }
}
