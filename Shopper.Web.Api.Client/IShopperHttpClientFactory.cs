using System.Net.Http;

namespace Shopper.Web.Api.Client
{
    internal interface IShopperHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}