using Chamados.DTOs.Users;

namespace Chamados.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto requestUser);

        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequest);

        Task<GetUserResponseDto> GetUserByEmailAsync(GetUserRequestDto getUserRequest);
    }
}
