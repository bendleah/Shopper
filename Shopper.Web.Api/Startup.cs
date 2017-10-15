using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shopper.Web.Api.Storage;
using Shopper.Web.Api.Models;
using AutoMapper;

namespace Shopper.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAutoMapper();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles("Shopper.Web.Api");
            });

            services.AddSingleton(x => config.CreateMapper());

            var productStore = new InMemoryDataStore<Product>();
            var basketStore = new InMemoryDataStore<Basket>();

            SeedProductStore(productStore);

            services.AddSingleton<IDataStore<Product>>(productStore);
            services.AddSingleton<IDataStore<Basket>>(basketStore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
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
