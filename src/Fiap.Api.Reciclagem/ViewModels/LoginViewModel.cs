using System.ComponentModel.DataAnnotations;

namespace Fiap.Api.Reciclagem.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
