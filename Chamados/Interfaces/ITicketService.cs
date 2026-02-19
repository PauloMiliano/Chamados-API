using Chamados.DTOs.Tickets;
using Chamados.Models;

namespace Chamados.Interfaces
{
    public interface ITicketService
    {
        Task<TicketResponse> OpenTicket(TicketRequestDto ticket, string id);

        Task<List<TicketListDto>> GetAllTickets(int pageNumber, int pageSize);

        Task<Ticket> GetTicketById(Guid id);
    }
}
