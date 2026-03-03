using Chamados.DTOs.Users;

namespace Chamados.Interfaces
{
    /// <summary>
    /// Provides operations for user authentication and registration.
    /// </summary>
    public interface IUserService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto requestUser);

        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequest);
    }
}
