using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shopper.Web.Api.Client.Models;

namespace Shopper.Web.Api.Client.Clients
{
    internal class ProductClient : ApiClient, IProductClient
    {
        public ProductClient(IShopperHttpClientFactory httpClientFactory) :
            base(httpClientFactory, new ApiUriBuilder("api/products"))
        { }

        public async Task<ApiResponse<IEnumerable<Product>>> GetAllProducts()
        {
            var requestUri = UriBuilder.Build();

            return await HttpGetAsync<IEnumerable<Product>>(requestUri);
        }

        public async Task<ApiResponse<Product>> GetProduct(Guid productId)
        {
            var requestUri = UriBuilder.Build(Uri.EscapeDataString(productId.ToString()));

            return await HttpGetAsync<Product>(requestUri);
        }
     
    }
}
