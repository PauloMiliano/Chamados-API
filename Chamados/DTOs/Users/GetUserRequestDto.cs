using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Users
{
    /// <summary>
    /// Represents a request to retrieve user information by email address.
    /// </summary>
    public class GetUserRequestDto
    {
        [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
        [EmailAddress(ErrorMessage = "Digite um endereço de e-mail válido.")]
        public string Email { get; set; }
    }
}
