using Microsoft.AspNetCore.Mvc.Testing;

using RESTFulSense.Clients;

namespace App.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private readonly WebApplicationFactory<Program> webApplicationFactory;
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient restFulApiFactoryClient;

        public ApiBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<Program>();
            this.httpClient = this.webApplicationFactory.CreateClient();
            this.restFulApiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);
        }
    }
}
