using Shopper.Web.Api.Models;
using Shopper.Web.Api.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopper.Web.Api.Client.Test
{
    public static class Seeder
    {

        public static void Seed(this IDataStore<Basket> baskets, IDataStore<Product> products)
        {
            var product1 = products.Get(new Guid("3f994e1b-fdf9-48a5-8d8c-e8c3fc9b8f0a"));
            var product2 = products.Get(new Guid("4a739ec8-81a2-4247-8098-eca377cc4de5"));

            if (product1 == null || product2 == null)
                return;

            baskets.TryAdd(new Basket()
            {
                Id = new Guid("ae81ca02-3a4a-4148-a4bf-30a733ff0387"),
                CreatedDate = DateTime.UtcNow,
                OrderLines = new Dictionary<Guid, OrderLine>()
                {
                    {
                        product1.Id ,
                        new OrderLine()
                        {
                            Product =  product1,
                            Quantity = 5
                        }
                    } ,
                    {
                        product2.Id ,
                        new OrderLine()
                        {
                            Product = product2 ,
                            Quantity = 8
                        }
                    }
                },
                OwnerId = new Guid("c197b8dc-274a-40a7-a843-bba68d4cec3f")
            });

            baskets.TryAdd(new Basket()
            {
                Id = new Guid("06847481-4e76-48d1-95d1-e054c846dbff"),
                CreatedDate = DateTime.UtcNow,
                OrderLines = new Dictionary<Guid, OrderLine>(),
                OwnerId = new Guid("6f2fe249-a24d-4f9f-9132-1bf2d5a406fc")
            });
        }

    }
}
