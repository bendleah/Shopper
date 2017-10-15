using System;
using System.Collections.Generic;
using System.Text;

namespace Shopper.Web.Api.Client
{
    internal class ApiUriBuilder
    {
        private readonly string _rootPath;
        public Uri Build()
        {
            return new Uri(_rootPath, UriKind.Relative);
        }

        public Uri Build(string path)
        {
            return new Uri(string.Format("{0}/{1}", _rootPath, path), UriKind.Relative);
        }

        public ApiUriBuilder(string rootPath)
        {
            _rootPath = rootPath;
        }
    }
}
