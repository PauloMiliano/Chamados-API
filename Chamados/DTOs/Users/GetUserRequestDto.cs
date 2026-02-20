using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Users
{
    public class GetUserRequestDto
    {
        [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
        [EmailAddress(ErrorMessage = "Digite um endereço de e-mail válido.")]
        public string Email { get; set; }
    }
}
