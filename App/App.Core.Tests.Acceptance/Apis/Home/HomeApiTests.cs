using App.Core.Tests.Acceptance.Brokers;
using FluentAssertions;

namespace App.Core.Tests.Acceptance.Apis.Home
{
    [Collection(nameof(ApiTestCollection))]
    public class HomeApiTests
    {
        private readonly ApiBroker apiBroker;

        public HomeApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        [Fact]
        public async Task ShouldReturnHomeMessageAsync()
        {
            // Given
            string expectedMessage = 
                "... The princess isn't here!...";

            // When
            string actualMessage =
                await this.apiBroker.GetHomeMessage();

            // Then
            actualMessage.Should().Be(expectedMessage);
        }
    }
}
