using Chamados.Data;
using Chamados.DTOs.Tickets;
using Chamados.Enums;
using Chamados.Exceptions;
using Chamados.Interfaces;
using Chamados.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;


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

        public async Task<TicketResponse> CloseTicket(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .Include(a => a.Author)
                .FirstOrDefaultAsync(u => u.Id == ticketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            ticket.Status = TicketStatus.Closed;
            await _context.SaveChangesAsync();

            return new TicketResponse
            {
                Id = ticket.Id,
                AuthorName = ticket.Author.Name,
                Title = ticket.Title,
                Priority = ticket.Priority,
                Status = ticket.Status,
                Description = ticket.Description,
                Date = ticket.Created
            };
        }

        public async Task<TicketResponse> AssignUserTicket (Guid ticketId, string userId)
        {
            var ticket = await _context.Tickets
                .Include(a => a.Author)
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(u => u.Id == ticketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            ticket.AssignedToUserId = userId;
            await _context.SaveChangesAsync();

            return new TicketResponse
            {
                Id = ticket.Id,
                AuthorName = ticket.Author.Name,
                Title = ticket.Title,
                Priority = ticket.Priority,
                Status = ticket.Status,
                Date = ticket.Created,
                AssignedToUserName = ticket.AssignedToUser.Name,
            };
        }

        public async Task<List<TicketListDto>> GetAllTickets(int pageNumber, int pageSize)
        {
            var tickets = await _context.Tickets
                    .Include(a => a.Author)
                    .Include(a => a.AssignedToUser)
                    .OrderBy(c => c.Created)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            var ticketList = new List<TicketListDto>();

            foreach (var ticket in tickets)
            {
                ticketList.Add(new TicketListDto
                {
                    TicketId = ticket.Id,
                    Title = ticket.Title,
                    AuthorId = ticket.AuthorId,
                    AuthorName = ticket.Author.Name,
                    Created = ticket.Created,
                    Status = ticket.Status,
                    AssignedToUserId = ticket.AssignedToUserId,
                    AssignedToUserName = ticket.AssignedToUser.Name,
                });
            }
            return ticketList;
        }

        public async Task<TicketResponse> GetTicketById(Guid id)
        {
            var ticket = await _context.Tickets
                .Include(a => a.Author)
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            return new TicketResponse
            {
                Id = ticket.Id,
                AuthorName = ticket.Author.Name,
                Title = ticket.Title,
                Priority = ticket.Priority,
                Status = ticket.Status,
                Description = ticket.Description,
                Date = ticket.Created,
                AssignedToUserName = ticket.AssignedToUser.Name
            };
        }
    }
}
