using Chamados.Data;
using Chamados.DTOs.Tickets;
using Chamados.Interfaces;
using Chamados.Models;
using Microsoft.AspNetCore.Identity;

namespace Chamados.Services
{
    public class TicketService : ITicketService
    {
        private readonly ILogger<TicketService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TicketService(ILogger<TicketService> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<TicketResponse> OpenTicket(TicketRequestDto ticketRequest)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(ticketRequest.AuthorId);
                var userName = user != null ? user.Name : "Desconhecido";

                var ticket = new Ticket
                {
                    Title = ticketRequest.Title,
                    Description = ticketRequest.Description,
                    AuthorId = ticketRequest.AuthorId,
                    Priority = ticketRequest.Priority,
                    Status = ticketRequest.Status,
                    Created = ticketRequest.Created
                };
                _context.Tickets.Add(ticket);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new TicketResponse
                    {
                        Message = $"Ticket aberto com sucesso.",
                        Title = ticket.Title,
                        Description = ticket.Description,
                        Success = true,
                        UserName = userName,
                        Date = ticket.Created
                    };
                }
                else
                {
                    return new TicketResponse
                    {
                        Message = $"Falha ao abrir ticket.",
                        Title = ticket.Title,
                        Description = ticket.Description,
                        Success = false,
                        UserName = userName,
                        Date = DateTime.Now
                    };
                }
            }
            catch (Exception ex)
            {
                return new TicketResponse
                {
                    Message = $"Ocorreu um erro ao abrir o ticket. Exceção: {ex.Message}",
                    Title = ticketRequest.Title,
                    Description = ticketRequest.Description,
                    Success = false
                };
            }
        }
    }
}
