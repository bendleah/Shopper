using Shopper.Web.Api.Client.Clients;

namespace Shopper.Web.Api.Client
{
    public interface IShopperApiClient
    {
        IBasketClient Baskets { get; }
        IProductClient Products { get; }
    }
}