﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Client.Dto
{
    public class OrderLineUpdateDto
    { 
        public int Quantity { get; set; }
    }
}
