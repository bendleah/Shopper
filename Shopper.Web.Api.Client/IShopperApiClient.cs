namespace Shopper.Web.Api.Client
{
    public interface IShopperApiClient
    {
        IBasketClient Baskets { get; }
    }
}