using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shopper.Web.Api.Client
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Reason { get; set; }
    }

    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Reason { get; set; }
        public T Data { get; set; }
    }
}
