using Chamados.Data;
using Chamados.DTOs.Users;
using Chamados.Interfaces;
using Chamados.Models;
using Chamados.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chamados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService loginService, ILogger<UserController> logger)
        {
            _userService = loginService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> UserLogin(LoginRequestDto requestUser)
        {
            try
            {
                _logger.LogInformation("Tentativa de login para Email: {Email}", requestUser.Email);
                var userResponse = await _userService.LoginAsync(requestUser);

                return Ok(userResponse);

            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao realizar login. Exceção: " + ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterRequestDto registerRequest)
        {
            try
            {
                _logger.LogInformation("Tentativa de registro para o email: {Email}", registerRequest.Email);
                var registerResponse = await _userService.RegisterAsync(registerRequest);

                return Ok(registerResponse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao registrar. Exceção: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("get")]
        public async Task<IActionResult> GetUserByEmail(GetUserRequestDto getUserRequest)
        {
            try
            {
                _logger.LogInformation("Tentativa de obter usuário para o email: {Email}", getUserRequest);
                var userResponse = await _userService.GetUserByEmailAsync(getUserRequest);
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter usuário. Exceção: {ex.Message}");
            }

        }
    }
}
