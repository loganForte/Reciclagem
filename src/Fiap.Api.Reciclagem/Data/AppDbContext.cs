using Fiap.Api.Reciclagem.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Api.Reciclagem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<PontoColeta> PontosColeta => Set<PontoColeta>();
        public DbSet<Descarte> Descartes => Set<Descarte>();
        public DbSet<Alerta> Alertas => Set<Alerta>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PontoColeta>().HasData(
                new PontoColeta
                {
                    PontoColetaId = 1,
                    Nome = "Ecoponto Vila Verde",
                    Endereco = "Rua das Flores, 100",
                    Bairro = "Vila Verde",
                    CapacidadeMaximaKg = 200,
                    CargaAtualKg = 0,
                    Ativo = true
                },
                new PontoColeta
                {
                    PontoColetaId = 2,
                    Nome = "Ecoponto Jardim Azul",
                    Endereco = "Av. Central, 500",
                    Bairro = "Jardim Azul",
                    CapacidadeMaximaKg = 150,
                    CargaAtualKg = 0,
                    Ativo = true
                }
            );
        }
    }
}
