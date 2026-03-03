using Chamados.DTOs.Tickets;
using Chamados.Enums;
using Chamados.Models;

namespace Chamados.Interfaces
{
    /// <summary>
    /// Provides operations for managing tickets.
    /// </summary>
    public interface ITicketService
    {
        Task<TicketResponse> OpenTicket(CreateTicketDto ticket, string userId);

        Task<CloseTicketDto> CloseTicket(Guid ticketId);

        Task<TicketActionsDto> AssignUserTicket(Guid ticketId, string userId);

        Task<List<TicketListDto>> GetAllTickets(int pageNumber, int pageSize, string userId, string userRole, TicketStatus? status);

        Task<TicketResponse> GetTicketById(Guid ticketId);

        Task<TicketActionsDto> ReopenTicket(Guid ticketId);

        Task<TicketActionsDto> ChangeTicketPriority(Guid ticketId, TicketPriority priority);

    }
}
