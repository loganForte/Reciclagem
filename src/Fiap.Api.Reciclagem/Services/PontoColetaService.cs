using Fiap.Api.Reciclagem.Data;
using Fiap.Api.Reciclagem.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Api.Reciclagem.Services
{
    public class PontoColetaService : IPontoColetaService
    {
        private readonly AppDbContext _context;

        public PontoColetaService(AppDbContext context) => _context = context;

        public IEnumerable<PontoColeta> Listar(int pagina, int tamanho)
        {
            if (pagina < 1) pagina = 1;
            if (tamanho < 1) tamanho = 10;

            return _context.PontosColeta
                .OrderBy(p => p.PontoColetaId)
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .AsNoTracking()
                .ToList();
        }

        public int ContarTotal() => _context.PontosColeta.Count();

        public PontoColeta? ObterPorId(int id) => _context.PontosColeta
            .Include(p => p.Descartes)
            .AsNoTracking()
            .FirstOrDefault(p => p.PontoColetaId == id);

        public PontoColeta Criar(PontoColeta pontoColeta)
        {
            _context.PontosColeta.Add(pontoColeta);
            _context.SaveChanges();
            return pontoColeta;
        }

        public bool Atualizar(int id, PontoColeta dados)
        {
            var existente = _context.PontosColeta.Find(id);
            if (existente == null) return false;

            existente.Nome = dados.Nome;
            existente.Endereco = dados.Endereco;
            existente.Bairro = dados.Bairro;
            existente.CapacidadeMaximaKg = dados.CapacidadeMaximaKg;
            existente.Ativo = dados.Ativo;

            _context.SaveChanges();
            return true;
        }

        public bool Remover(int id)
        {
            var existente = _context.PontosColeta.Find(id);
            if (existente == null) return false;

            _context.PontosColeta.Remove(existente);
            _context.SaveChanges();
            return true;
        }
    }
}
