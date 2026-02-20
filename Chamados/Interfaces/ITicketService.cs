using Chamados.DTOs.Tickets;
using Chamados.Models;

namespace Chamados.Interfaces
{
    public interface ITicketService
    {
        Task<TicketResponse> OpenTicket(CreateTicketDto ticket, string userId);

        Task<TicketResponse> AssignUserTicket(AssignTicketDto assignTicket, string userId);

        Task<List<TicketListDto>> GetAllTickets(int pageNumber, int pageSize);

        Task<Ticket> GetTicketById(Guid id);
    }
}
