using Shopper.Web.Api.Client.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopper.Web.Api.Client
{
    public class ShopperApiClient : IShopperApiClient
    {
        public IBasketClient Baskets  { get; }
        public ShopperApiClient(ApiSettings apiSettings)
        {
            var factory = new ShopperHttpClientFactory(apiSettings);
            Baskets = new BasketClient(factory);
        }
    }
}
