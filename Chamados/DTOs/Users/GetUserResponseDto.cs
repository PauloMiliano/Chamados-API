using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Users
{
    public class GetUserResponseDto
    {
        [EmailAddress(ErrorMessage = "Digite um endereço de e-mail válido.")]
        [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O Id do usuário é obrigatório.")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "A função do usuário é obrigatória.")]
        public List<string> Roles { get; set; }
    }
}
