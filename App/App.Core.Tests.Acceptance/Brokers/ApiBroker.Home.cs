namespace App.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string HomeRelativeUrl = "api/home";

        public async ValueTask<string> GetHomeMessage() =>
            await this.restFulApiFactoryClient.GetContentStringAsync(HomeRelativeUrl);
    }
}
