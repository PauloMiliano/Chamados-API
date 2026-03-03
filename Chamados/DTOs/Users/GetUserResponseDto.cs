using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Users
{
    /// <summary>
    /// Represents the response containing user information, including email, user ID, username, and roles.
    /// </summary>
    public class GetUserResponseDto
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
