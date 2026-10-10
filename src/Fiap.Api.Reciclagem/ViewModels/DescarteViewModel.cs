using System.ComponentModel.DataAnnotations;

namespace Fiap.Api.Reciclagem.ViewModels
{
    public class DescarteViewModel
    {
        public int DescarteId { get; set; }
        public int PontoColetaId { get; set; }
        public string TipoResiduo { get; set; } = string.Empty;
        public double PesoKg { get; set; }
        public DateTime DataDescarte { get; set; }
    }

    public class DescarteCreateViewModel
    {
        [Required(ErrorMessage = "O ponto de coleta é obrigatório.")]
        public int PontoColetaId { get; set; }

        [Required(ErrorMessage = "O tipo de resíduo é obrigatório.")]
        public string TipoResiduo { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O peso deve ser maior que zero.")]
        public double PesoKg { get; set; }
    }
}
