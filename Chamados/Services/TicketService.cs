using Chamados.Data;
using Chamados.DTOs.Tickets;
using Chamados.Interfaces;
using Chamados.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


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

        public async Task<TicketResponse> OpenTicket(TicketRequestDto ticketRequest, string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var userName = user != null ? user.Name : "Desconhecido";

                var ticket = new Ticket
                {
                    Title = ticketRequest.Title,
                    Description = ticketRequest.Description,
                    Priority = ticketRequest.Priority,
                    AuthorId = id,
                    Status = ticketRequest.Status,
                    Created = ticketRequest.Created
                };
                _context.Tickets.Add(ticket);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new TicketResponse
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        Description = ticket.Description,
                        AuthorName = userName,
                        Date = ticket.Created
                    };
                }
                else
                {
                    return new TicketResponse
                    {
                        Title = ticket.Title,
                        Description = ticket.Description,
                        AuthorName = userName,
                        Date = DateTime.Now
                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<TicketListDto>> GetAllTickets(int pageNumber, int pageSize)
        {
            try
            {
                var tickets = await _context.Tickets
                    .OrderBy(c => c.Created)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                
                return tickets.Select(ticket => new TicketListDto
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    AuthorId = ticket.AuthorId,
                    AuthorName = _userManager.FindByIdAsync(ticket.AuthorId).Result.Name,
                    Created = ticket.Created,
                    Status = ticket.Status,
                    AssignedToUserId = ticket.AssignedToUserId,
                    AssignedToUserName = !string.IsNullOrEmpty(ticket.AssignedToUserId) ? _userManager.FindByIdAsync(ticket.AssignedToUserId).Result.Name : null
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter tickets");
                return new List<TicketListDto>();
            }
        }

        public async Task<Ticket> GetTicketById(Guid id)
        {
            try
            {
                return await _context.Tickets.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                return new Ticket
                {
                    Title = $"Ocorreu um erro ao obter o ticket. Exceção: {ex.Message}"
                };
            }
        }
    }
}
