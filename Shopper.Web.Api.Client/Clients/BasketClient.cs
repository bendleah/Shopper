using Microsoft.AspNetCore.JsonPatch;
using Shopper.Web.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Client.Clients
{
    internal class BasketClient : ApiClient, IBasketClient
    {
        public BasketClient(IShopperHttpClientFactory httpClientFactory) :
            base(httpClientFactory, new ApiUriBuilder("api/baskets"))
        {}


        public async Task<ApiResponse<Basket>> CreateBasket(Guid ownerId)
        {
            var requestUri = UriBuilder.Build();

            return await HttpPostAsync<Basket, dynamic>(requestUri, new
            {
                OwnerId = ownerId
            });
        }

        public async Task<ApiResponse<IEnumerable<Basket>>> GetAllBaskets()
        {
            var requestUri = UriBuilder.Build();

            return await HttpGetAsync<IEnumerable<Basket>>(requestUri);
        }

        public async Task<ApiResponse<Basket>> GetBasket(Guid basketId)
        {
            var requestUri = UriBuilder.Build(Uri.EscapeDataString(basketId.ToString()));

            return await HttpGetAsync<Basket>(requestUri);
        }

        public async Task<ApiResponse> DeleteBasket(Guid basketId)
        {
            var requestUri = UriBuilder.Build(Uri.EscapeDataString(basketId.ToString()));

            return await HttpDeleteAsync(requestUri);
        }

        public async Task<ApiResponse<IEnumerable<OrderLine>>> GetOrderLines(Guid basketId)
        {
            var requestUri = UriBuilder.Build(string.Format("{0}/orderlines", Uri.EscapeDataString(basketId.ToString())));

            return await HttpGetAsync<IEnumerable<OrderLine>>(requestUri);
        }

        public async Task<ApiResponse<OrderLine>> GetOrderLine(Guid basketId, Guid productId)
        {
            var requestUri = UriBuilder.Build(string.Format("{0}/orderlines/{1}", 
                                                            Uri.EscapeDataString(basketId.ToString()) ,
                                                            Uri.EscapeDataString(productId.ToString())));

            return await HttpGetAsync<OrderLine>(requestUri);
        }

        public async Task<ApiResponse<OrderLine>> CreateOrderLine(Guid basketId, Guid productId, int quantity)
        {
            var requestUri = UriBuilder.Build(string.Format("{0}/orderlines", Uri.EscapeDataString(basketId.ToString())));

            return await HttpPostAsync<OrderLine, dynamic>(requestUri, new
            {
                ProductId = productId ,
                Quantity = quantity
            });
        }

        public async Task<ApiResponse<OrderLine>> UpdateOrderLine(Guid basketId, Guid productId, int newQuantity)
        {
            var requestUri = UriBuilder.Build(string.Format("{0}/orderlines/{1}",
                                                            Uri.EscapeDataString(basketId.ToString()),
                                                            Uri.EscapeDataString(productId.ToString())));

            var patch = new JsonPatchDocument().Add("/quantity", newQuantity);

            return await HttpPatchAsync<OrderLine, JsonPatchDocument>(requestUri, patch);
        }

        public async Task<ApiResponse<Basket>> ClearBasket(Guid basketId)
        {
            var requestUri = UriBuilder.Build(string.Format("{0}/clear", Uri.EscapeDataString(basketId.ToString())));

            return await HttpPutAsync<Basket, dynamic>(requestUri, new
            {
                BasketId = basketId
            });
        }
    }
}
