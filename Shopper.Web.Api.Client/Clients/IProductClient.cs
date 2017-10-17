using Shopper.Web.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Client.Clients
{
    public interface IProductClient
    {
        Task<ApiResponse<IEnumerable<Product>>> GetAllProducts();
        Task<ApiResponse<Product>> GetProduct(Guid productId);
    }
}
