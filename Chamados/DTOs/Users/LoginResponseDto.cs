namespace Chamados.DTOs.Users
{
    /// <summary>
    /// Represents the response returned after a successful login, containing the JWT token for authentication.
    /// </summary>
    public class LoginResponseDto
    {
        public string Token { get; set; }
    }
}
