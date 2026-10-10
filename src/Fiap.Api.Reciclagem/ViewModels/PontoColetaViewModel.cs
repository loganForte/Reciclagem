using System.ComponentModel.DataAnnotations;

namespace Fiap.Api.Reciclagem.ViewModels
{
    public class PontoColetaViewModel
    {
        public int PontoColetaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public double CapacidadeMaximaKg { get; set; }
        public double CargaAtualKg { get; set; }
        public bool Ativo { get; set; }
    }

    public class PontoColetaCreateViewModel
    {
        [Required(ErrorMessage = "O nome do ponto de coleta é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Endereco { get; set; } = string.Empty;

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; } = string.Empty;

        [Range(1, double.MaxValue, ErrorMessage = "A capacidade máxima deve ser maior que zero.")]
        public double CapacidadeMaximaKg { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
