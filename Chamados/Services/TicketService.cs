using Chamados.Data;
using Chamados.DTOs.Tickets;
using Chamados.Exceptions;
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

        public async Task<TicketResponse> OpenTicket(CreateTicketDto ticketRequest, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userName = user != null ? user.Name : "Desconhecido";

            var ticket = new Ticket
            {
                Title = ticketRequest.Title,
                Description = ticketRequest.Description,
                Priority = ticketRequest.Priority,
                AuthorId = userId,
                Status = ticketRequest.Status,
                Created = ticketRequest.Created
            };
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return new TicketResponse
            {
                Id = ticket.Id,
                AuthorName = userName,
                Title = ticket.Title,
                Priority = ticket.Priority,
                Status = ticket.Status,
                Description = ticket.Description,
                Date = ticket.Created
            };
        }

        public async Task<TicketResponse> AssignUserTicket (AssignTicketDto assignTicket, string userId)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(u => u.Id == assignTicket.TicketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            ticket.AssignedToUserId = userId;
            await _context.SaveChangesAsync();

            var userNameAnalyst = _userManager.FindByIdAsync(userId).Result.Name;

            return new TicketResponse
            {
                Id = ticket.Id,
                AuthorName = _userManager.FindByIdAsync(ticket.AuthorId).Result.Name,
                Title = ticket.Title,
                Priority = ticket.Priority,
                Status = ticket.Status,
                Description = ticket.Description,
                Date = ticket.Created,
                AssignedToUserName = userNameAnalyst
            };

        }

        public async Task<List<TicketListDto>> GetAllTickets(int pageNumber, int pageSize)
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

        public async Task<Ticket> GetTicketById(Guid id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(u => u.Id == id);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            return ticket;
        }
    }
}
