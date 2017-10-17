using Shopper.Web.Api.Models;
using Shopper.Web.Api.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopper.Web.Api.Test
{
    public static class Seeder
    {

        public static void Seed(this IDataStore<Product> store)
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
