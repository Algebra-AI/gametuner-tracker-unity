using System;
using System.Net.Http;

namespace SnowplowTracker.Requests
{
    internal class HttpRequest
    {
        public Enums.HttpMethod Method { get; }
        public Uri CollectorUri { get; }
        public HttpContent Content { get; }
        public string StringContent { get; }

        public HttpRequest(Enums.HttpMethod method, Uri collectorUri, HttpContent content = null, string stringContent = "")
        {
            Method = method;
            CollectorUri = collectorUri;
            Content = content;
            StringContent = stringContent;
        }
    }
}
