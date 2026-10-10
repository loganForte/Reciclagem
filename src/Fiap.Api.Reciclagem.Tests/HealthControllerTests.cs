using Microsoft.AspNetCore.Mvc.Testing;

namespace Fiap.Api.Reciclagem.Tests
{
    public class HealthTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public HealthTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Health_ReturnsHealthy()
        {
            var response = await _client.GetAsync("/health");

            response.EnsureSuccessStatusCode();
            Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
        }
    }
}
