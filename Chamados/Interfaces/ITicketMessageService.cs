using Chamados.DTOs.TicketsMessages;
using Chamados.Models;

namespace Chamados.Interfaces
{
    /// <summary>
    /// Provides operations for managing ticket messages.
    /// </summary>
    public interface ITicketMessageService
    {
        Task<TicketMessageResponseDto> CreateMessageAsync(Guid ticketId, string senderId, CreateTicketMessageDto message);
        Task<List<TicketMessageResponseDto>> GetMessagesAsync(Guid ticketId);
    }
}
