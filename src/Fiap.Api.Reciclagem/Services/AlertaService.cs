using Fiap.Api.Reciclagem.Data;
using Fiap.Api.Reciclagem.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Api.Reciclagem.Services
{
    public class AlertaService : IAlertaService
    {
        private readonly AppDbContext _context;

        public AlertaService(AppDbContext context) => _context = context;

        public IEnumerable<Alerta> Listar(int pagina, int tamanho, bool? resolvido)
        {
            if (pagina < 1) pagina = 1;
            if (tamanho < 1) tamanho = 10;

            var query = _context.Alertas.AsQueryable();

            if (resolvido.HasValue)
                query = query.Where(a => a.Resolvido == resolvido.Value);

            return query
                .OrderByDescending(a => a.DataGeracao)
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .AsNoTracking()
                .ToList();
        }

        public Alerta? ObterPorId(int id) => _context.Alertas
            .Include(a => a.PontoColeta)
            .AsNoTracking()
            .FirstOrDefault(a => a.AlertaId == id);

        public bool Resolver(int id)
        {
            var alerta = _context.Alertas.Find(id);
            if (alerta == null) return false;

            alerta.Resolvido = true;

            var ponto = _context.PontosColeta.Find(alerta.PontoColetaId);
            if (ponto != null)
                ponto.CargaAtualKg = 0;

            _context.SaveChanges();
            return true;
        }
    }
}
