using Microsoft.AspNetCore.Mvc.Testing;

namespace Fiap.Api.Reciclagem.Tests
{
    public class AlertaControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AlertaControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnsHttpStatusCode200()
        {
            // Arrange
            var request = "/api/Alerta";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
