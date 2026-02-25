using Chamados.DTOs.Tickets;
using Chamados.Enums;
using Chamados.Models;

namespace Chamados.Interfaces
{
    public interface ITicketService
    {
        Task<TicketResponse> OpenTicket(CreateTicketDto ticket, string userId);

        Task<CloseTicketDto> CloseTicket(Guid ticketId);

        Task<AssignTicketDto> AssignUserTicket(Guid ticketId, string userId);

        Task<List<TicketListDto>> GetAllTickets(int pageNumber, int pageSize, string userId, string userRole, TicketStatus? status);

        Task<TicketResponse> GetTicketById(Guid ticketId);
    }
}
