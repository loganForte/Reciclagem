namespace Fiap.Api.Reciclagem.Models
{
    public class Descarte
    {
        public int DescarteId { get; set; }
        public int PontoColetaId { get; set; }
        public PontoColeta? PontoColeta { get; set; }

        // Papel, Plastico, Vidro, Metal ou Organico
        public string TipoResiduo { get; set; } = string.Empty;
        public double PesoKg { get; set; }
        public DateTime DataDescarte { get; set; } = DateTime.UtcNow;
    }
}
