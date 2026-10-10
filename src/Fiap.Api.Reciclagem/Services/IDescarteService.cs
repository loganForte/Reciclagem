using Fiap.Api.Reciclagem.Models;

namespace Fiap.Api.Reciclagem.Services
{
    public interface IDescarteService
    {
        IEnumerable<Descarte> Listar(int pagina, int tamanho, int? pontoColetaId);
        Descarte? ObterPorId(int id);

        // Registra o descarte, atualiza a carga do ponto de coleta e gera
        // um alerta automatico caso o limite configurado seja atingido.
        (Descarte descarte, Alerta? alertaGerado) RegistrarDescarte(Descarte descarte);
    }
}
