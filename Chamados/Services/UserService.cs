using Azure.Identity;
using Chamados.Data;
using Chamados.DTOs.Users;
using Chamados.Exceptions;
using Chamados.Interfaces;
using Chamados.Models;
using Chamados.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chamados.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto requestUser)
        {
            var user = await _userManager.FindByEmailAsync(requestUser.Email);

            if (user == null)
            {
                _logger.LogWarning("Login falhou: usuário não encontrado para email {Email}", requestUser.Email);
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            var login = await _signInManager.CheckPasswordSignInAsync(user, requestUser.Password, lockoutOnFailure: true);

            if (!login.Succeeded)
            {
                _logger.LogWarning("Login falhou: senha inválida para email {Email}", requestUser.Email);
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            if (login.IsLockedOut)
            {
                throw new UnauthorizedAccessException("Usuário bloqueado devido a múltiplas tentativas de login falhadas. Tente novamente mais tarde.");
            }

            var token = await _tokenService.GenerateToken(user);

            _logger.LogInformation("Login realizado com sucesso para usuário: {UserName}", user.Name);

            return new LoginResponseDto
            {
                Token = token
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
        {
            var user = new User
            {
                UserName = registerRequest.Email,
                Name = registerRequest.Name,
                Email = registerRequest.Email
            };

            var newUser = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!newUser.Succeeded)
            {
                var errors = string.Join(", ", newUser.Errors.Select(e => e.Description));

                throw new BadRequestException(errors);
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, registerRequest.Role);

            if (!addRoleResult.Succeeded)
            {
                var erros = addRoleResult.Errors.Select(e => e.Description);
                throw new ApplicationException($"Erro ao registrar usuário: {string.Join(", ")}");
            }

            var token = await _tokenService.GenerateToken(user);

            return new RegisterResponseDto
            {
                UserName = registerRequest.Name,
                Token = token
            };
        }

        public async Task<GetUserResponseDto> GetUserByEmailAsync(GetUserRequestDto getUserRequest)
        {
            var user = await _userManager.FindByEmailAsync(getUserRequest.Email);
            var roles = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            return new GetUserResponseDto
            {
                Email = user.Email,
                UserId = user.Id,
                UserName = user.Name,
                Roles = roles.ToList()
            };
        }

    }
}
