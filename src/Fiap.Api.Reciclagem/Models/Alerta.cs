namespace Fiap.Api.Reciclagem.Models
{
    public class Alerta
    {
        public int AlertaId { get; set; }
        public int PontoColetaId { get; set; }
        public PontoColeta? PontoColeta { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
        public bool Resolvido { get; set; } = false;
    }
}
