namespace Fiap.Api.Reciclagem.Models
{
    public class PontoColeta
    {
        public int PontoColetaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public double CapacidadeMaximaKg { get; set; }
        public double CargaAtualKg { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<Descarte> Descartes { get; set; } = new List<Descarte>();
        public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
    }
}
