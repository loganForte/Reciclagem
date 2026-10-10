using System.Net.Http.Json;
using Fiap.Api.Reciclagem.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Fiap.Api.Reciclagem.Tests
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ReturnsHttpStatusCode200()
        {
            // Arrange
            var request = "/api/Auth/login";
            var login = new LoginViewModel { Username = "gestor01", Password = "pass123" };

            // Act
            var response = await _client.PostAsJsonAsync(request, login);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
