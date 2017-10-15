using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Shopper.Web.Api.Client
{
    internal class ShopperHttpClientFactory : IShopperHttpClientFactory
    {
        private readonly ApiSettings _settings;

        public ShopperHttpClientFactory(ApiSettings settings)
        {
            _settings = settings;
        }

        public HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = _settings.BaseUri
            };

            return httpClient;

        }
    }
}
