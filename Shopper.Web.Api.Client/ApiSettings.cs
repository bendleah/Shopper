using System;
using System.Collections.Generic;
using System.Text;

namespace Shopper.Web.Api.Client
{
    public class ApiSettings 
    {
        public Uri BaseUri { get; }

        public ApiSettings(Uri baseUri)
        {
  
            BaseUri = baseUri;
        }
    }

}
