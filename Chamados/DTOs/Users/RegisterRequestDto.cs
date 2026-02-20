using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Users
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
        [EmailAddress(ErrorMessage = "Digite um endereço de e-mail válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo senha é obrigatório.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve conter entre 6 a 20 carácteres.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "A função do usuário é obrigatória.")]
        public string Role { get; set; }
    }
}
