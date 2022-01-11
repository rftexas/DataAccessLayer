using System.Collections.Generic;
using System.Net.Http;

namespace DataAccessLayer {
    public class EndpointConfiguration {
        public string BaseUrl { get; init; }

        public List<DelegatingHandler> InnerHandlers{ get; init; }
    }

    public class EndpointConfiguration<TEndpoint> : EndpointConfiguration { }
}