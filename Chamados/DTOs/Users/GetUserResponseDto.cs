using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Users
{
    public class GetUserResponseDto
    {
        public string Message { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public List<string> Roles { get; set; }
    }
}
