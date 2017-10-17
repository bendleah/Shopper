using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shopper.Web.Api.Client.Models;
using Shopper.Web.Api.Storage;
using Shopper.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Client.Test
{
    [TestClass]
    public class BasketClientTest
    {
        private IWebHost _server;
        private IShopperApiClient _client;

        private Guid _testBasketId = new Guid("ae81ca02-3a4a-4148-a4bf-30a733ff0387");
        private Guid _testProductId = new Guid("4a739ec8-81a2-4247-8098-eca377cc4de5");

        [TestInitialize]
        public void Initialize()
        {
            var uri = "http://localhost:5050"; //TODO: Move to config

            _server = new WebHostBuilder().UseKestrel().UseStartup<Startup>().UseUrls(uri).Build();

            var basketStore = (IDataStore<Api.Models.Basket>)_server.Services.GetService(typeof(IDataStore<Api.Models.Basket>));
            var productStore = (IDataStore<Api.Models.Product>)_server.Services.GetService(typeof(IDataStore<Api.Models.Product>));

            SeedBasketStore(basketStore, productStore);

            _server.Start();
            _client = new ShopperApiClient(new ApiSettings(new Uri(uri)));

        }


        [TestMethod]
        public async Task BasketClient_Get_All_Baskets_Return_Correct_Number()
        {
            var baskets = await _client.Baskets.GetAllBaskets();

            Assert.IsNotNull(baskets);

            Assert.IsTrue(baskets.Success);

            Assert.IsNotNull(baskets.Data);

            Assert.IsTrue(new List<Api.Client.Models.Basket>(baskets.Data).Count == 2);
        }

        [TestMethod]
        public async Task BasketClient_Get_Single_Baskets_Return_Basket()
        {
            var basket = await _client.Baskets.GetBasket(_testBasketId);

            Assert.IsNotNull(basket);

            Assert.IsTrue(basket.Success);

            Assert.IsNotNull(basket.Data);

            Assert.IsTrue(basket.Data.Id == _testBasketId);
        }


        [TestMethod]
        public async Task BasketClient_Create_New_Basket_Return_Created_Basket()
        {
            var newBasket = await _client.Baskets.CreateBasket(Guid.NewGuid());

            Assert.IsNotNull(newBasket);

            Assert.IsTrue(newBasket.Success);

            Assert.IsNotNull(newBasket.Data);
        }

        [TestMethod]
        public async Task BasketClient_Get_Basket_Order_Lines_Return_Correct_Number()
        {
            var orderlines = await _client.Baskets.GetOrderLines(_testBasketId);

            Assert.IsNotNull(orderlines);

            Assert.IsTrue(orderlines.Success);

            Assert.IsNotNull(orderlines.Data);

            Assert.IsTrue(new List<Models.OrderLine>(orderlines.Data).Count == 2);
        }

        [TestMethod]
        public async Task BasketClient_Get_Basket_Order_Line_Return_Correct_Order_Line()
        {
            var orderline = await _client.Baskets.GetOrderLine(_testBasketId, _testProductId);

            Assert.IsNotNull(orderline);

            Assert.IsTrue(orderline.Success);

            Assert.IsNotNull(orderline.Data);

            Assert.IsTrue(orderline.Data.ProductId == _testProductId);
        }

        [TestMethod]
        public async Task BasketClient_Create_Basket_Order_Line_Return_Correct_Order_Line()
        {
            var orderline = await _client.Baskets.CreateOrderLine(_testBasketId, new Guid("3bd6360d-9358-43c2-b9a9-237815797c83"), 12);

            Assert.IsNotNull(orderline);

            Assert.IsTrue(orderline.Success);

            Assert.IsNotNull(orderline.Data);

            Assert.IsTrue(orderline.Data.Quantity == 12);
        }

        [TestMethod]
        public async Task BasketClient_Update_Basket_Order_Line_Return_Correct_Updated_Order_Line()
        {
            var orderline = await _client.Baskets.UpdateOrderLine(_testBasketId, _testProductId, 22);

            Assert.IsNotNull(orderline);

            Assert.IsTrue(orderline.Success);

            Assert.IsNotNull(orderline.Data);

            Assert.IsTrue(orderline.Data.Quantity == 22);
        }

        [TestMethod]
        public async Task BasketClient_Clear_Basket_Check_Basket_Clear()
        {
            var basket = await _client.Baskets.ClearBasket(_testBasketId);

            Assert.IsNotNull(basket);

            Assert.IsTrue(basket.Success);

            Assert.IsNotNull(basket.Data);

            Assert.IsTrue(basket.Data.NumberOfOrderLines == 0);
        }

        [TestMethod]
        public async Task BasketClient_Delete_Basket_Check_Success()
        {
            var response = await _client.Baskets.DeleteBasket(_testBasketId);

            Assert.IsNotNull(response);

            Assert.IsTrue(response.Success);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _server.Dispose();
        }


        private void SeedBasketStore(IDataStore<Api.Models.Basket> basketStore, IDataStore<Api.Models.Product> productStore)
        {
            var product1 = productStore.Get(new Guid("3f994e1b-fdf9-48a5-8d8c-e8c3fc9b8f0a"));
            var product2 = productStore.Get(new Guid("4a739ec8-81a2-4247-8098-eca377cc4de5"));

            basketStore.TryAdd(new Api.Models.Basket()
            {
                Id = _testBasketId,
                CreatedDate = DateTime.UtcNow,
                OrderLines = new Dictionary<Guid, Api.Models.OrderLine>()
                {
                    {
                        product1.Id ,
                        new Api.Models.OrderLine()
                        {
                            Product =  product1,
                            Quantity = 5
                        }
                    } ,
                    {
                        product2.Id ,
                        new Api.Models.OrderLine()
                        {
                            Product = product2 ,
                            Quantity = 8
                        }
                    }
                },
                OwnerId = new Guid("c197b8dc-274a-40a7-a843-bba68d4cec3f")
            });

            basketStore.TryAdd(new Api.Models.Basket()
            {
                Id = new Guid("06847481-4e76-48d1-95d1-e054c846dbff"),
                CreatedDate = DateTime.UtcNow,
                OrderLines = new Dictionary<Guid, Api.Models.OrderLine>(),
                OwnerId = new Guid("6f2fe249-a24d-4f9f-9132-1bf2d5a406fc")
            });
        }
    }
}
