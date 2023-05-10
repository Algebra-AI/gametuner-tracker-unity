using System;
using System.Net.Http;

namespace GametunerTracker.Requests
{
    internal class HttpRequest
    {
        public Enums.HttpMethod Method { get; }
        public Uri CollectorUri { get; }
        public HttpContent Content { get; }

        public HttpRequest(Enums.HttpMethod method, Uri collectorUri, HttpContent content = null)
        {
            Method = method;
            CollectorUri = collectorUri;
            Content = content;
        }
    }
}
