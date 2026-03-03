using Chamados.Models;

namespace Chamados.Interfaces
{
    /// <summary>
    /// Provides operations for generating authentication tokens.
    /// </summary>
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
    }
}
