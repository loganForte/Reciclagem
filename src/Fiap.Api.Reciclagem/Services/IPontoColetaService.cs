using Fiap.Api.Reciclagem.Models;

namespace Fiap.Api.Reciclagem.Services
{
    public interface IPontoColetaService
    {
        IEnumerable<PontoColeta> Listar(int pagina, int tamanho);
        int ContarTotal();
        PontoColeta? ObterPorId(int id);
        PontoColeta Criar(PontoColeta pontoColeta);
        bool Atualizar(int id, PontoColeta pontoColeta);
        bool Remover(int id);
    }
}
