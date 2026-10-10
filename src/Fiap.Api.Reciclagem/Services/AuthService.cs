using Fiap.Api.Reciclagem.Models;

namespace Fiap.Api.Reciclagem.Services
{
    public class AuthService : IAuthService
    {
        private readonly List<UserModel> _usuarios = new()
        {
            new UserModel { UserId = 1, Username = "operador01", Password = "pass123", Role = "operador" },
            new UserModel { UserId = 2, Username = "gestor01", Password = "pass123", Role = "gestor" }
        };

        public UserModel? Authenticate(string username, string password) =>
            _usuarios.FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}
