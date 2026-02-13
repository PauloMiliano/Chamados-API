using Chamados.Data;
using Chamados.DTOs.Users;
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
            try
            {
                var user = await _userManager.FindByEmailAsync(requestUser.Email);

                if (user == null)
                {
                    _logger.LogInformation("Usuário não encontrado para email: {Email}", requestUser.Email);
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Usuário ou senha inválidos."
                    };
                }

                var passwordValid = await _userManager.CheckPasswordAsync(user, requestUser.Password);

                if (!passwordValid)
                {
                    _logger.LogInformation("Senha inválida.");
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Usuário ou senha inválidos."
                    };
                }

                var token = await _tokenService.GenerateToken(user);

                _logger.LogInformation("Login realizado com sucesso para usuário: {UserName}", user.Name);

                return new LoginResponseDto
                {
                    Success = true,
                    Message = "Login realizado com sucesso.",
                    Token = token
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Erro durante login para email: {Email}", requestUser.Email);
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Ocorreu um erro ao realizar login. Tente novamente mais tarde.",
                };
            }
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
        {
            try
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
                    return new RegisterResponseDto
                    {
                        Success = false,
                        Errors = newUser.Errors.Select(e => e.Description),
                        Message = "Erro ao registrar usuário."
                    };
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, registerRequest.Role);

                if (!addRoleResult.Succeeded)
                {
                    return new RegisterResponseDto
                    {
                        Success = false,
                        Message = "Erro ao adicionar a role ao usuário."
                    };
                }

                return new RegisterResponseDto
                {
                    Success = true,
                    Message = "Registro realizado com sucesso."
                };

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Erro durante registro para email: {Email}", registerRequest.Email);
                return null;
            }
        }

    }
}
