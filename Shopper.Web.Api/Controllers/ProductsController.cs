using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopper.Web.Api.Storage;
using Shopper.Web.Api.Models;
using AutoMapper;
using Shopper.Web.Api.Dto;

namespace Shopper.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private IDataStore<Product> _products;
        private IMapper _mapper;

        public ProductsController(IDataStore<Product> products, IMapper mapper)
        {
            _products = products;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all availible products
        /// </summary>
        [HttpGet(Name = "GetAllProducts")]
        public IActionResult GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(_products.GetAll()));
        }

        /// <summary>
        /// Get an individual product
        /// </summary>
        [HttpGet("{id:guid}", Name ="GetProductById")]
        public IActionResult Get(Guid id)
        {
            if (!_products.Contains(id)) NotFound();

            return Ok(_mapper.Map<ProductDto>( _products.Get(id)));
        }
    }
}
