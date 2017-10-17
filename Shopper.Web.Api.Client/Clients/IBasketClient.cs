using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopper.Web.Api.Client.Models;

namespace Shopper.Web.Api.Client.Clients
{
    public interface IBasketClient
    {

        Task<ApiResponse<Basket>> CreateBasket(Guid ownerId);
        Task<ApiResponse<Basket>> ClearBasket(Guid basketId);
        Task<ApiResponse<Basket>> GetBasket(Guid basketId);
        Task<ApiResponse> DeleteBasket(Guid basketId);
        Task<ApiResponse<IEnumerable<Basket>>> GetAllBaskets();
        Task<ApiResponse<OrderLine>> GetOrderLine(Guid basketId, Guid productId);
        Task<ApiResponse<IEnumerable<OrderLine>>> GetOrderLines(Guid basketId);
        Task<ApiResponse<OrderLine>> CreateOrderLine(Guid basketId, Guid productId, int quantity);
        Task<ApiResponse<OrderLine>> UpdateOrderLine(Guid basketId, Guid productId, int newQuantity);
    }
}