using Chamados.Data;
using Chamados.DTOs.Tickets;
using Chamados.Enums;
using Chamados.Exceptions;
using Chamados.Interfaces;
using Chamados.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


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
                Created = DateTime.UtcNow
            };
            await _context.Tickets.AddAsync(ticket);

            var ticketHistory = new TicketHistory
            {
                TicketId = ticket.Id,
                Action = TicketActions.Opened,
                PerformedByUserId = userId,
                PerformedAt = DateTime.UtcNow
            };
            await _context.TicketHistories.AddAsync(ticketHistory);
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

        public async Task<CloseTicketDto> CloseTicket(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .Include(a => a.Author)
                .FirstOrDefaultAsync(u => u.Id == ticketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            var ticketHistory = new TicketHistory
            {
                TicketId = ticket.Id,
                Action = TicketActions.Closed,
                PerformedByUserId = ticket.Author.Id,
                PerformedAt = DateTime.UtcNow
            };

            await _context.TicketHistories.AddAsync(ticketHistory);
            ticket.Status = TicketStatus.Closed;
            await _context.SaveChangesAsync();

            return new CloseTicketDto
            {
                Id = ticket.Id,
                AuthorName = ticket.Author.Name,
                Title = ticket.Title,
                Status = ticket.Status,
                Date = DateTime.UtcNow
            };
        }

        public async Task<AssignTicketDto> AssignUserTicket (Guid ticketId, string userId)
        {
            var ticket = await _context.Tickets
                .Include(a => a.Author)
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(u => u.Id == ticketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            var ticketHistory = new TicketHistory
            {
                TicketId = ticket.Id,
                Action = TicketActions.Assigned,
                PerformedByUserId = ticket.Author.Id,
                PerformedAt = DateTime.UtcNow
            };

            await _context.TicketHistories.AddAsync(ticketHistory);
            ticket.AssignedToUserId = userId;
            ticket.Status = TicketStatus.InProgress;
            await _context.SaveChangesAsync();

            return new AssignTicketDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                AuthorName = ticket.Author.Name,
                AssignedToUserName = ticket.AssignedToUser.Name,
                Status = ticket.Status,
                Date = ticket.Created,
            };
        }

        public async Task<List<TicketListDto>> GetAllTickets(int pageNumber, int pageSize, string userId, string userRole, TicketStatus? status)
        {
            var tickets = _context.Tickets
                        .Include(a => a.Author)
                        .Include(a => a.AssignedToUser)
                        .OrderByDescending(c => c.Created)
                        .AsQueryable();

            if (userRole == "User")
            {
                tickets = tickets.Where(t => t.AuthorId == userId);
            }

            if (status.HasValue)
            {
                tickets = tickets.Where(t => t.Status == status.Value);
            }

            var ticketList = await tickets
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TicketListDto
                {
                    TicketId = t.Id,
                    Title = t.Title,
                    AuthorName = t.Author.Name,
                    Created = t.Created,
                    Status = t.Status,
                    AssignedToUserName = t.AssignedToUser != null ? t.AssignedToUser.Name : null
                })
                .ToListAsync();
            return ticketList;
        }

        public async Task<TicketResponse> GetTicketById(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .Include(a => a.Author)
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

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
