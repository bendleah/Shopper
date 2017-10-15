using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Client.Clients
{
    internal abstract class ApiClient
    {
        private readonly IShopperHttpClientFactory _httpClientFactory;
        public readonly ApiUriBuilder UriBuilder;

        public ApiClient(IShopperHttpClientFactory httpClientFactory, ApiUriBuilder uriBuilder)
        {
            _httpClientFactory = httpClientFactory;
            UriBuilder = uriBuilder;
        }

        public async Task<ApiResponse<T>> HttpGetAsync<T>(Uri requestUri)
        {
            return await GetApiResponse<T>(requestUri, "GET");
        }

        public async Task<ApiResponse<T>> HttpPutAsync<T, U>(Uri requestUri, U content)
        {
            return await GetApiResponse<T, U>(requestUri, content, "PUT");
        }

        public async Task<ApiResponse<T>> HttpPatchAsync<T, U>(Uri requestUri, U content)
        {
            return await GetApiResponse<T, U>(requestUri, content, "PATCH");
        }

        public async Task<ApiResponse<T>> HttpPostAsync<T, U>(Uri requestUri, U content)
        {
            return await GetApiResponse<T, U>(requestUri, content, "POST");
        }

        public async Task<ApiResponse<T>> HttpDeleteAsync<T>(Uri requestUri)
        {
            return await GetApiResponse<T>(requestUri, "DELETE");
        }

        private async Task<ApiResponse<T>> GetApiResponse<T>(Uri requestUri, string httpVerb)
        {
            using (var httpClient = _httpClientFactory.CreateHttpClient())
            {
                httpClient.Timeout = new TimeSpan(0, 0, 30);

                var request = new HttpRequestMessage(new HttpMethod(httpVerb), requestUri);

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                return await FormatResponse<T>(response);
            }
        }

        private async Task<ApiResponse<T>> GetApiResponse<T, U>(Uri requestUri, U content, string httpVerb)
        {
            using (var httpClient = _httpClientFactory.CreateHttpClient())
            {
                httpClient.Timeout = new TimeSpan(0, 0, 30);

                var request = new HttpRequestMessage(new HttpMethod(httpVerb), requestUri);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                return await FormatResponse<T>(response);
            }
        }

        private async Task<ApiResponse<T>> FormatResponse<T>(HttpResponseMessage httpResponse)
        {
            string data = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var apiResponse = new ApiResponse<T>()
            {
                StatusCode = httpResponse.StatusCode,
                Reason = httpResponse.ReasonPhrase,
                Success = httpResponse.IsSuccessStatusCode
            };

            if (apiResponse.Success)
            {
                try
                {
                    T result = (T)JsonConvert.DeserializeObject(data, typeof(T));
                    apiResponse.Data = result;
                }
                catch (Exception ex)
                {
                    apiResponse.Success = false;
                    apiResponse.Data = default(T);
                    apiResponse.Reason = string.Format("Unable to deserialize response to {0}", typeof(T).ToString());
                }
            }
            else
            {
                apiResponse.Data = default(T);
            }

            return apiResponse;
        }
    }
}
}
