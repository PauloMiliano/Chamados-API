using Chamados.DTOs.TicketsMessages;
using Chamados.Models;

namespace Chamados.Interfaces
{
    public interface ITicketMessageService
    {
        Task<TicketMessageResponseDto> CreateMessageAsync(Guid ticketId, string senderId, CreateTicketMessageDto message);
        Task<List<TicketMessageResponseDto>> GetMessagesAsync(Guid ticketId);
    }
}
