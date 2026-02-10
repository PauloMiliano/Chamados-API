using Chamados.Models;

namespace Chamados.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
    }
}
