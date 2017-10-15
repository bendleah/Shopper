using AutoMapper;
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
        public void Get_All_Baskets_Return_Correct_Number()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            SeedProductStore(products);
            SeedBasketStore(baskets);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.GetAllBaskets();

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            List<BasketDto> resultBaskets = okresult.Value as List<BasketDto>;

            Assert.IsNotNull(resultBaskets);

            Assert.AreEqual(resultBaskets.Count, baskets.Count());
        }

        [TestMethod]
        public void Get_Single_Basket_Return_Correct_Basket()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            SeedProductStore(products);
            SeedBasketStore(baskets);

            var bc = new BasketsController(products, baskets, _mapper);

            var id = baskets.ToList()[0].Id;

            var actionResult = bc.GetBasketById(id);

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            BasketDto resultBasket = okresult.Value as BasketDto;

            Assert.IsNotNull(resultBasket);

            Assert.AreEqual(resultBasket.Id, id);

        }


        [TestMethod]
        public void Create_New_Basket_Return_Correct_Basket()
        {
            var baskets = new InMemoryDataStore<Basket>();
            var products = new InMemoryDataStore<Product>();

            SeedProductStore(products);
            SeedBasketStore(baskets);

            var bc = new BasketsController(products, baskets, _mapper);

            var actionResult = bc.PostCreateBasket(new BasketCreateDto()
            {
                OwnerId = Guid.NewGuid()
            });

            OkObjectResult okresult = actionResult as OkObjectResult;

            Assert.IsNotNull(okresult);

            BasketDto resultBasket = okresult.Value as BasketDto;

            Assert.IsNotNull(resultBasket);
        }



        private void SeedBasketStore(IDataStore<Basket> store)
        {
            store.TryAdd(new Basket()
            {
                Id = new Guid("ae81ca02-3a4a-4148-a4bf-30a733ff0387"),
                CreatedDate = DateTime.UtcNow,
                OrderLines = new Dictionary<Guid, OrderLine>(),
                OwnerId = new Guid("c197b8dc-274a-40a7-a843-bba68d4cec3f")
            });

            store.TryAdd(new Basket()
            {
                Id = new Guid("06847481-4e76-48d1-95d1-e054c846dbff"),
                CreatedDate = DateTime.UtcNow,
                OrderLines = new Dictionary<Guid, OrderLine>(),
                OwnerId = new Guid("6f2fe249-a24d-4f9f-9132-1bf2d5a406fc")
            });
        }

        private void SeedProductStore(IDataStore<Product> store)
        {
            store.TryAdd(
                new Product()
                {
                    Id = new Guid("3f994e1b-fdf9-48a5-8d8c-e8c3fc9b8f0a"),
                    Name = "Orange",
                    Description = "A very orange orange.",
                    Price = 0.99M
                }
            );

            store.TryAdd(
                new Product()
                {
                    Id = new Guid("f7ae77da-7fab-4e7d-98c8-964564a67d0c"),
                    Name = "Apple",
                    Description = "Crisp and juicy.",
                    Price = 0.59M
                }
            );

            store.TryAdd(
                new Product()
                {
                    Id = new Guid("4a739ec8-81a2-4247-8098-eca377cc4de5"),
                    Name = "Banana",
                    Description = "Ripen at home.",
                    Price = 0.39M
                }
            );

            store.TryAdd(
                  new Product()
                  {
                      Id = new Guid("3bd6360d-9358-43c2-b9a9-237815797c83"),
                      Name = "Peach",
                      Description = "Real peachy.",
                      Price = 0.89M
                  }
            );

            store.TryAdd(
                new Product()
                {
                    Id = new Guid("3aee1758-77b9-4e86-9c57-77adbbc0956f"),
                    Name = "Watermelon",
                    Description = "Just add vodka.",
                    Price = 1.29M
                }
           );
        }
    }
}
