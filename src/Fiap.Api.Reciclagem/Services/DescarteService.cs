using Fiap.Api.Reciclagem.Data;
using Fiap.Api.Reciclagem.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Api.Reciclagem.Services
{
    public class DescarteService : IDescarteService
    {
        private readonly AppDbContext _context;
        private const double PercentualAlerta = 0.8;

        public DescarteService(AppDbContext context) => _context = context;

        public IEnumerable<Descarte> Listar(int pagina, int tamanho, int? pontoColetaId)
        {
            if (pagina < 1) pagina = 1;
            if (tamanho < 1) tamanho = 10;

            var query = _context.Descartes.AsQueryable();

            if (pontoColetaId.HasValue)
                query = query.Where(d => d.PontoColetaId == pontoColetaId.Value);

            return query
                .OrderByDescending(d => d.DataDescarte)
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .AsNoTracking()
                .ToList();
        }

        public Descarte? ObterPorId(int id) => _context.Descartes
            .Include(d => d.PontoColeta)
            .AsNoTracking()
            .FirstOrDefault(d => d.DescarteId == id);

        public (Descarte descarte, Alerta? alertaGerado) RegistrarDescarte(Descarte descarte)
        {
            var ponto = _context.PontosColeta.FirstOrDefault(p => p.PontoColetaId == descarte.PontoColetaId);
            if (ponto == null)
                throw new InvalidOperationException("Ponto de coleta não encontrado.");

            descarte.DataDescarte = DateTime.UtcNow;
            _context.Descartes.Add(descarte);

            ponto.CargaAtualKg += descarte.PesoKg;

            Alerta? alertaGerado = null;
            var limite = ponto.CapacidadeMaximaKg * PercentualAlerta;

            if (ponto.CargaAtualKg >= limite)
            {
                var jaExisteAlertaAberto = _context.Alertas
                    .Any(a => a.PontoColetaId == ponto.PontoColetaId && !a.Resolvido);

                if (!jaExisteAlertaAberto)
                {
                    alertaGerado = new Alerta
                    {
                        PontoColetaId = ponto.PontoColetaId,
                        Mensagem = $"Ponto '{ponto.Nome}' atingiu {ponto.CargaAtualKg:N1}kg de {ponto.CapacidadeMaximaKg:N1}kg. Coleta necessária.",
                        DataGeracao = DateTime.UtcNow,
                        Resolvido = false
                    };
                    _context.Alertas.Add(alertaGerado);
                }
            }

            _context.SaveChanges();
            return (descarte, alertaGerado);
        }
    }
}
