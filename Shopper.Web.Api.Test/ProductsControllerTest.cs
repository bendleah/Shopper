using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shopper.Web.Api.Controllers;
using Shopper.Web.Api.Dto;
using Shopper.Web.Api.Models;
using Shopper.Web.Api.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopper.Web.Api.Test
{
    [TestClass]
    public class ProductsControllerTest
    {
        private IMapper _mapper;
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
        public void ProductsController_Get_All_Products_Return_Correct_Number()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();

            var pc = new ProductsController(products, _mapper);

            var actionResult = pc.GetAll();

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            List<ProductDto> resultProducts = okresult.Value as List<ProductDto>;

            Assert.IsNotNull(resultProducts);

            Assert.AreEqual(resultProducts.Count, 5);
        }

        [TestMethod]
        public void ProductsController_Get_Product_Return_Product()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            products.Seed();

            var pc = new ProductsController(products, _mapper);

            var actionResult = pc.Get(new Guid("4a739ec8-81a2-4247-8098-eca377cc4de5"));

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            ProductDto resultProduct = okresult.Value as ProductDto;

            Assert.IsNotNull(resultProduct);
        }
    }
}
