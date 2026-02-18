using Chamados.DTOs.Tickets;
using Chamados.Models;

namespace Chamados.Interfaces
{
    public interface ITicketService
    {
        Task<TicketResponse> OpenTicket(TicketRequestDto ticket);
    }
}
