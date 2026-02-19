using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Users
{
    public class GetUserRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
