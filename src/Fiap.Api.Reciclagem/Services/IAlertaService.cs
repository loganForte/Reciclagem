using Fiap.Api.Reciclagem.Models;

namespace Fiap.Api.Reciclagem.Services
{
    public interface IAlertaService
    {
        IEnumerable<Alerta> Listar(int pagina, int tamanho, bool? resolvido);
        Alerta? ObterPorId(int id);
        bool Resolver(int id);
    }
}
