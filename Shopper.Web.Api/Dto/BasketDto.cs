using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Dto
{
    public class BasketDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedDate { get; set; }

        public int NumberOfOrderLines { get; set; }
    }
}
