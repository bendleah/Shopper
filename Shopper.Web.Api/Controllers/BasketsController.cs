using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopper.Web.Api.Models;
using Shopper.Web.Api.Storage;
using System.ComponentModel.DataAnnotations;
using Shopper.Web.Api.Dto;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace Shopper.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class BasketsController : Controller
    {
        private IDataStore<Product> _products;
        private IDataStore<Basket> _baskets;
        private IMapper _mapper;

        public BasketsController(IDataStore<Product> products, IDataStore<Basket> baskets, IMapper mapper)
        {
            _products = products;
            _baskets = baskets;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all baskets
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllBaskets")]
        public IActionResult GetAllBaskets()
        {
            return Ok(_mapper.Map<List<BasketDto>>(_baskets.GetAll()));
        }

        /// <summary>
        /// Get a basket by id
        /// </summary>
        [HttpGet("{id:guid}", Name = "GetBasketById")]
        public IActionResult GetBasketById(Guid id)
        {
            if (!_baskets.Contains(id)) return NotFound(id);

            return Ok(_mapper.Map<BasketDto>(_baskets.Get(id)));
        }

        /// <summary>
        /// Delete a basket by id
        /// </summary>
        [HttpDelete("{id:guid}", Name = "DeleteBasketById")]
        public IActionResult DeleteBasketById(Guid id)
        {
            if (!_baskets.Contains(id)) return NotFound(id);

            if (!_baskets.Remove(id)) return new BadRequestResult();

            return Ok();
        }

        /// <summary>
        /// Get all order lines for a given basket 
        /// </summary>
        [HttpGet("{id:guid}/orderlines", Name = "GetBasketOrderLines")]
        public IActionResult GetBasketOrderLines(Guid id)
        {
            if (!_baskets.Contains(id)) return NotFound(id);

            var orderlines = _baskets.Get(id).OrderLines.Values;

            return Ok(_mapper.Map<IEnumerable<OrderLineDto>>(orderlines));
        }

        /// <summary>
        /// Get an order line for a given basket
        /// </summary>
        [HttpGet("{id:guid}/orderlines/{productId:guid}", Name = "GetBasketOrderLine")]
        public IActionResult GetBasketOrderLine(Guid id, Guid productId)
        {
            if (!_baskets.Contains(id)) return NotFound(id);

            var basket = _baskets.Get(id);

            if (!basket.OrderLines.ContainsKey(productId)) return NotFound(productId);
            
            return Ok(_mapper.Map<OrderLineDto>(basket.OrderLines[productId]));
        }

        /// <summary>
        /// Add an order line to a basket
        /// </summary>
        [HttpPost("{id:guid}/orderlines", Name = "AddOrderLineToBasket")]
        public IActionResult PostAddOrderLineToBasket(Guid id, [FromBody]OrderLineCreateDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_baskets.Contains(id)) return NotFound(id);

            if (!_products.Contains(model.ProductId)) return NotFound(model.ProductId);

            var basket = _baskets.Get(id);
            var product = _products.Get(model.ProductId);
            var orderLine = new OrderLine()
            {
                Product = product,
                Quantity = model.Quantity
            };

            //Check to see if we already have an orderline for this product
            if (basket.OrderLines.ContainsKey(model.ProductId))
            {
                //If we already have an orderline for this product, add the new quantity to the existing order line
                basket.OrderLines[model.ProductId].Quantity += orderLine.Quantity;
                //return the updated orderline
                orderLine = basket.OrderLines[model.ProductId];
            }
            else
            {
                basket.OrderLines.Add(model.ProductId, orderLine);
            }

            return CreatedAtRoute("GetBasketOrderLine", new { id = id, productId = model.ProductId }, _mapper.Map<OrderLineDto>(orderLine));
        }


        /// <summary>
        /// Update an order line for a given basket.
        /// </summary>
        [HttpPatch("{id:guid}/orderlines/{productId:guid}", Name="UpdateOrderLine")]
        public IActionResult PatchOrderLine(Guid id, Guid productId, [FromBody]JsonPatchDocument<OrderLineUpdateDto> model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_baskets.Contains(id)) return NotFound(id);

            var bucket = _baskets.Get(id);

            if (!bucket.OrderLines.ContainsKey(productId)) return NotFound(productId);

            var orderline = bucket.OrderLines[productId];

            var dto = _mapper.Map<OrderLineUpdateDto>(orderline);

            //Apply the patch to the dto
            model.ApplyTo(dto);

            //Map the updated dto back to the model
            var updated = _mapper.Map(dto, orderline);

            if (updated.Quantity > 0)
            {
                //Set the updated orderline
                bucket.OrderLines[productId] = updated;
            }
            else
            {
                //Remove the orderline if the quantity is less than 1
                bucket.OrderLines.Remove(productId);
            }

            return Ok(_mapper.Map<OrderLineDto>(updated));
        }


        /// <summary>
        /// Clear all orderlines from a basket
        /// </summary>
        [HttpPut("{id:guid}/clear", Name="ClearBasket")]
        public IActionResult PutClearBasket(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_baskets.Contains(id)) return NotFound(id);

            var basket = _baskets.Get(id);

            basket.OrderLines.Clear();

            return Ok(_mapper.Map<BasketDto>(basket));
        }       

        /// <summary>
        /// Create a basket for a given user
        /// </summary>
        [HttpPost(Name = "CreateBasket")]
        public IActionResult PostCreateBasket([FromBody]BasketCreateDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Guid g = Guid.NewGuid();
            var basket = new Basket()
            {
                Id = g,
                CreatedDate = DateTime.UtcNow,
                OwnerId = model.OwnerId
            };

            if (!_baskets.TryAdd(basket)) return new BadRequestResult();

            return CreatedAtRoute("GetBasketById", new { id = g }, _mapper.Map<BasketDto>(basket));
        }

    }
}
