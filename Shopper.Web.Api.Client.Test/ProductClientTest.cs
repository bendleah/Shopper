using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shopper.Web.Api.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Client.Test
{

    [TestClass]
    public class ProductClientTest
    {
        private IWebHost _server;
        private IShopperApiClient _client;

        private Guid _testProductId = new Guid("4a739ec8-81a2-4247-8098-eca377cc4de5");

        [TestInitialize]
        public void Initialize()
        {
            var uri = "http://localhost:5050"; //TODO: Move to config
            _server = new WebHostBuilder().UseKestrel().UseStartup<Startup>().UseUrls(uri).Build();
            _server.Start();
            _client = new ShopperApiClient(new ApiSettings(new Uri(uri)));
        }


        [TestMethod]
        public async Task ProductClient_Get_All_Products_Return_Correct_Number()
        {
            var products = await _client.Products.GetAllProducts();

            Assert.IsNotNull(products);

            Assert.IsTrue(products.Success);

            Assert.IsNotNull(products.Data);

            Assert.IsTrue(new List<Api.Client.Models.Product>(products.Data).Count == 5);
        }

        [TestMethod]
        public async Task ProductClient_Get_Single_Product_Return_Product()
        {
            var product = await _client.Products.GetProduct(_testProductId);

            Assert.IsNotNull(product);

            Assert.IsTrue(product.Success);

            Assert.IsNotNull(product.Data);

            Assert.IsTrue(product.Data.Name == "Banana");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _server.Dispose();
        }
    }
}
