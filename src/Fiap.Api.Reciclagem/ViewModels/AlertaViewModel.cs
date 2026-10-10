namespace Fiap.Api.Reciclagem.ViewModels
{
    public class AlertaViewModel
    {
        public int AlertaId { get; set; }
        public int PontoColetaId { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataGeracao { get; set; }
        public bool Resolvido { get; set; }
    }
}
