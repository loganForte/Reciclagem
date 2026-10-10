using Fiap.Api.Reciclagem.Models;

namespace Fiap.Api.Reciclagem.Services
{
    public interface IAuthService
    {
        UserModel? Authenticate(string username, string password);
    }
}
