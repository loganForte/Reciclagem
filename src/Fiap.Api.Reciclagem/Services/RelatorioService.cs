using Fiap.Api.Reciclagem.Data;

namespace Fiap.Api.Reciclagem.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly AppDbContext _context;

        public RelatorioService(AppDbContext context) => _context = context;

        public object ResumoGeral()
        {
            var porTipoResiduo = _context.Descartes
                .GroupBy(d => d.TipoResiduo)
                .Select(g => new { TipoResiduo = g.Key, TotalKg = g.Sum(d => d.PesoKg) })
                .OrderByDescending(g => g.TotalKg)
                .ToList();

            var porPonto = _context.PontosColeta
                .Select(p => new
                {
                    p.PontoColetaId,
                    p.Nome,
                    p.CargaAtualKg,
                    p.CapacidadeMaximaKg,
                    PercentualOcupado = p.CapacidadeMaximaKg == 0
                        ? 0
                        : Math.Round(p.CargaAtualKg / p.CapacidadeMaximaKg * 100, 1)
                })
                .ToList();

            return new
            {
                TotalColetadoKg = _context.Descartes.Sum(d => (double?)d.PesoKg) ?? 0,
                AlertasAbertos = _context.Alertas.Count(a => !a.Resolvido),
                PorTipoResiduo = porTipoResiduo,
                PorPontoColeta = porPonto
            };
        }
    }
}
