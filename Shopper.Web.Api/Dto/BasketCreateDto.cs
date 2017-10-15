using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Dto
{
    public class BasketCreateDto
    {
        [Required]
        public Guid OwnerId { get; set; }
    }
}
