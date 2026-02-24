using Chamados.Data;
using Chamados.DTOs.Tickets;
using Chamados.Enums;
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

        public async Task<List<TicketListDto>> GetAllTickets(int pageNumber, int pageSize, TicketStatus? status)
        {
            var tickets = new List<Ticket>();

            if (status.HasValue)
            {
                tickets = await _context.Tickets
                        .Include(a => a.Author)
                        .Include(a => a.AssignedToUser)
                        .OrderBy(c => c.Created)
                        .Where(t => t.Status == status)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
            }
            else
            {
                tickets = await _context.Tickets
                        .Include(a => a.Author)
                        .Include(a => a.AssignedToUser)
                        .OrderBy(c => c.Created)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
            }

            var ticketList = new List<TicketListDto>();

            foreach (var ticket in tickets)
            {
                var assignedToUserName = ticket.AssignedToUser != null ? ticket.AssignedToUser.Name : null;

                ticketList.Add(new TicketListDto
                {
                    TicketId = ticket.Id,
                    Title = ticket.Title,
                    AuthorId = ticket.AuthorId,
                    AuthorName = ticket.Author.Name,
                    Created = ticket.Created,
                    Status = ticket.Status,
                    AssignedToUserId = ticket.AssignedToUserId,
                    AssignedToUserName = assignedToUserName,
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
