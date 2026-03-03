namespace Chamados.DTOs.Users
{
    /// <summary>
    /// Represents the response returned after a successful user registration, containing the user's name and a JWT token for authentication.
    /// </summary>
    public class RegisterResponseDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
