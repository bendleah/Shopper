using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shopper.Web.Api.Controllers;
using Shopper.Web.Api.Dto;
using Shopper.Web.Api.Models;
using Shopper.Web.Api.Storage;
using System;
using System.Collections.Generic;

namespace Shopper.Web.Api.Test
{
    [TestClass]
    public class BasketsControllerTest
    {
        private IMapper _mapper;
        private Guid _testBasketId = new Guid("ae81ca02-3a4a-4148-a4bf-30a733ff0387");
        private Guid _testProductId = new Guid("4a739ec8-81a2-4247-8098-eca377cc4de5");

        [TestInitialize]
        public void Initialise()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles("Shopper.Web.Api");
            });

            _mapper = config.CreateMapper();
            
        }

        [TestMethod]
        public void BasketsController_Get_All_Baskets_Return_Correct_Number()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();
            baskets.Seed(products);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.GetAllBaskets();

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            List<BasketDto> resultBaskets = okresult.Value as List<BasketDto>;

            Assert.IsNotNull(resultBaskets);

            Assert.AreEqual(resultBaskets.Count, baskets.Count());
        }

        [TestMethod]
        public void BasketsController_Get_Single_Basket_Return_Correct_Basket()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();
            baskets.Seed(products);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.GetBasketById(_testBasketId);

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            BasketDto resultBasket = okresult.Value as BasketDto;

            Assert.IsNotNull(resultBasket);

            Assert.AreEqual(resultBasket.Id, _testBasketId);
        }

        [TestMethod]
        public void BasketsController_Delete_Basket_Return_Success()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();
            baskets.Seed(products);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.DeleteBasketById(_testBasketId);

            OkResult okresult = actionResult as OkResult;

            Assert.IsNotNull(okresult);
        }


        [TestMethod]
        public void BasketsController_Create_New_Basket_Return_Created_Basket()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();
            baskets.Seed(products);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.PostCreateBasket(new BasketCreateDto()
            {
                OwnerId = Guid.NewGuid()
            });

            CreatedAtRouteResult createdresult = actionResult as CreatedAtRouteResult;

            Assert.IsNotNull(createdresult);

            BasketDto resultBasket = createdresult.Value as BasketDto;

            Assert.IsNotNull(resultBasket);
        }

        [TestMethod]
        public void BasketsController_Get_Basket_Order_Lines_Return_Correct_Number()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();
            baskets.Seed(products);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.GetBasketOrderLines(_testBasketId);

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            List<OrderLineDto> resultOrderLines = okresult.Value as List<OrderLineDto>;

            Assert.IsNotNull(resultOrderLines);

            Assert.AreEqual(resultOrderLines.Count, 2);
        }

        [TestMethod]
        public void BasketsController_Get_Basket_Order_Line_Return_Correct_Order_Line()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();
            baskets.Seed(products);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.GetBasketOrderLine(_testBasketId, _testProductId);

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            OrderLineDto resultOrderLine = okresult.Value as OrderLineDto;

            Assert.IsNotNull(resultOrderLine);

            Assert.AreEqual(resultOrderLine.ProductId, _testProductId);
        }

        [TestMethod]
        public void BasketsController_Create_Basket_Order_Line_Return_Correct_Order_Line()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();
            baskets.Seed(products);

            var orderLine = new OrderLineCreateDto()
            {
                ProductId = new Guid("3aee1758-77b9-4e86-9c57-77adbbc0956f"), //Watermelon 
                Quantity = 5
            };

            var bc = new BasketsController(products, baskets, _mapper);
            var actionResult = bc.PostAddOrderLineToBasket(_testBasketId, orderLine);

            CreatedAtRouteResult createdresult = actionResult as CreatedAtRouteResult;

            Assert.IsNotNull(createdresult);

            OrderLineDto resultOrderLine = createdresult.Value as OrderLineDto;

            Assert.IsNotNull(resultOrderLine);

            Assert.AreEqual(resultOrderLine.ProductId, orderLine.ProductId);
        }

        [TestMethod]
        public void BasketsController_Update_Basket_Order_Line_Return_Correct_Updated_Order_Line()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();
            baskets.Seed(products);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.PatchOrderLine(_testBasketId, _testProductId, new JsonPatchDocument<OrderLineUpdateDto>().Add(p => p.Quantity, 12));

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            OrderLineDto resultOrderLine = okresult.Value as OrderLineDto;

            Assert.IsNotNull(resultOrderLine);

            Assert.IsTrue(resultOrderLine.Quantity == 12);

            OrderLine orderline = null;
            baskets.Get(_testBasketId)?.OrderLines.TryGetValue(resultOrderLine.ProductId, out orderline);

            Assert.IsNotNull(orderline);

            Assert.IsTrue(orderline.Quantity == 12);
        }

        [TestMethod]
        public void BasketsController_Clear_Basket_Check_Basket_Clear()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();
            baskets.Seed(products);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.PutClearBasket(_testBasketId);

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            BasketDto basket = okresult.Value as BasketDto;

            Assert.IsNotNull(basket);

            Assert.IsTrue(basket.NumberOfOrderLines == 0);
        }
    }
}
