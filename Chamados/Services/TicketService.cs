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

        /// <summary>
        /// Create a new ticket and associates it with the authenticated user.
        /// Also records the creation action in the ticket history.
        /// </summary>
        /// <param name="ticketRequest">Contains the required information to create a ticket, 
        /// such as title, description, priority, and status.
        /// </param>
        /// <param name="userId">Identifier of the user responsible for creating the ticket.</param>
        /// <returns>The created ticket data.</returns>
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

        /// <summary>
        /// Close an existing ticket by its identifier. Updates the ticket status to "Closed" and records the action in the ticket history.
        /// </summary>
        /// <param name="ticketId">Identifier of ticket to be closed</param>
        /// <returns>
        /// The updated ticket data after being closed.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown when the specified ticket is not found in the database.
        /// </exception>
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

        /// <summary>
        /// Assigns a ticket to a user by updating the "AssignedToUserId" property of the ticket and changing its status to "InProgress".
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket to be assigned.
        /// </param>
        /// <param name="userId">
        /// Identifier of the user who will be assigned to the ticket.
        /// </param>
        /// <returns>
        /// The updated ticket data after being assigned to the user, including the new status and assigned user information.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown when the specified ticket is not found in the database.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the ticket cannot be assigned due to its current status.
        /// </exception>
        public async Task<TicketActionsDto> AssignUserTicket (Guid ticketId, string userId)
        {
            var ticket = await _context.Tickets
                .Include(a => a.Author)
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(u => u.Id == ticketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                throw new InvalidOperationException("Um ticket fechado não pode ser atribuído.");
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

            return new TicketActionsDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                AuthorName = ticket.Author.Name,
                AssignedToUserName = ticket.AssignedToUser.Name,
                Status = ticket.Status,
                Date = ticket.Created,
            };
        }


        /// <summary>
        /// Reopens a closed ticket by updating its status back to "Open" and recording the action in the ticket history.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket to be reopened.
        /// </param>
        /// <returns>
        /// The updated ticket data after being reopened, including the new status and relevant information about the ticket.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown when the specified ticket is not found in the database.
        /// </exception>
        public async Task<TicketActionsDto> ReopenTicket(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .Include(a => a.Author)
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(u => u.Id == ticketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            ticket.Status = TicketStatus.Open;

            var ticketHistory = new TicketHistory
            {
                TicketId = ticket.Id,
                Action = TicketActions.Reopened,
                PerformedByUserId = ticket.Author.Id,
                PerformedAt = DateTime.UtcNow
            };
            await _context.TicketHistories.AddAsync(ticketHistory);
            await _context.SaveChangesAsync();

            return new TicketActionsDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                AuthorName = ticket.Author.Name,
                AssignedToUserName = ticket.AssignedToUser.Name
            };
        }

        /// <summary>
        /// Changes the priority of a ticket by updating its "Priority" property and recording the action in the ticket history.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket for which the priority will be changed.
        /// </param>
        /// <param name="priority">
        /// Priority level to be assigned to the ticket.
        /// </param>
        /// <returns>
        /// The updated ticket data after changing its priority, including the new priority level and relevant information about the ticket.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown when the specified ticket is not found in the database.
        /// </exception>
        public async Task<TicketActionsDto> ChangeTicketPriority(Guid ticketId, TicketPriority priority)
        {
            var ticket = await _context.Tickets
                .Include(a => a.Author)
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(u => u.Id == ticketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado");
            }

            ticket.Priority = priority;

            var ticketHistory = new TicketHistory
            {
                TicketId = ticket.Id,
                Action = TicketActions.PriorityChanged,
                PerformedByUserId = ticket.Author.Id,
                PerformedAt = DateTime.UtcNow
            };

            await _context.TicketHistories.AddAsync(ticketHistory);
            await _context.SaveChangesAsync();

            return new TicketActionsDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                AuthorName = ticket.Author.Name,
                AssignedToUserName = ticket.AssignedToUser.Name,
                Status = ticket.Status,
                Date = ticket.Created
            };
        }

        /// <summary>
        /// Gets a paginated list of tickets based on the provided parameters, including filtering by user role and ticket status.
        /// </summary>
        /// <param name="pageNumber">
        /// Page number for pagination, indicating which page of results to retrieve.
        /// </param>
        /// <param name="pageSize">
        /// Page size for pagination, indicating how many tickets to include in each page of results.
        /// </param>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="userRole">
        /// Role of the user making the request.
        /// </param>
        /// <param name="status">
        /// Status filter for the tickets.
        /// </param>
        /// <returns>
        /// The all tickets data based on the provided parameters, including pagination information and relevant details about each ticket.
        /// </returns>
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

        /// <summary>
        /// Gets a ticket by its identifier.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket to be retrieved.
        /// </param>
        /// <returns>
        /// A ticket data based on the provided identifier,
        /// including relevant details about the ticket such as title, description, author, assigned user, priority, status, and creation date.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown when the specified ticket is not found in the database.
        /// </exception>
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
