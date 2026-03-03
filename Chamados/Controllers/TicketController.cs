using Chamados.DTOs.Tickets;
using Chamados.Enums;
using Chamados.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chamados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <summary>
        /// Opens a new ticket with the provided details.
        /// </summary>
        /// <param name="ticket">
        /// Data required to create a new ticket.
        /// </param>
        [Authorize(Roles = "User,Analyst,Admin")]
        [HttpPost("open")]
        public async Task<IActionResult> OpenTicket([FromBody] CreateTicketDto ticket)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticketResponse = await _ticketService.OpenTicket(ticket, userId);
            return Ok(ticketResponse);
        }

        /// <summary>
        /// Gets a paginated list of tickets, optionally filtered by status. 
        /// Admins and Analysts can see all tickets, while Users can only see their own tickets.
        /// </summary>
        /// <param name="pageNumber">
        /// Page number for pagination (starting from 1).
        /// </param>
        /// <param name="pageSize">
        /// Page size for pagination (number of tickets per page).
        /// </param>
        /// <param name="status">
        /// Ticket status to filter by (optional). If not provided, all tickets are returned.
        /// </param>
        [Authorize(Roles = "Admin,Analyst,User")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTickets([FromQuery] int pageNumber, int pageSize, TicketStatus? status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var tickets = await _ticketService.GetAllTickets(pageNumber, pageSize, userId, userRole, status);
            return Ok(tickets);
        }

        /// <summary>
        /// Gets the details of a specific ticket by its ID.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket to retrieve.
        /// </param>
        [Authorize(Roles = "Admin,Analyst")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById([FromRoute] Guid ticketId)
        {
            var ticket = await _ticketService.GetTicketById(ticketId);
            return Ok(ticket);
        }

        /// <summary>
        /// Assigns the current user to the specified ticket.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket to which the user will be assigned.
        /// </param>
        [Authorize(Roles = "Admin,Analyst")]
        [HttpPut("assign/{ticketId}")]
        public async Task<IActionResult> AssignUserTicket([FromRoute] Guid ticketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticketResponse = await _ticketService.AssignUserTicket(ticketId, userId);
            return Ok(ticketResponse);
        }

        /// <summary>
        /// Closes the specified ticket, changing its status to "Closed".
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket to be closed.
        /// </param>
        [Authorize(Roles = "Admin,Analyst")]
        [HttpPut("close/{ticketId}")]
        public async Task<IActionResult> CloseTicket([FromRoute] Guid ticketId)
        {
            var ticketResponse = await _ticketService.CloseTicket(ticketId);
            return Ok(ticketResponse);
        }

        /// <summary>
        /// Reopens the specified ticket, changing its status back to "Open".
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket to be reopened.
        /// </param>
        [Authorize(Roles = "Admin,Analyst")]
        [HttpPut("reopen/{ticketId}")]
        public async Task<IActionResult> ReopenTicket([FromRoute] Guid ticketId)
        {
            var ticketResponse = await _ticketService.ReopenTicket(ticketId);
            return Ok(ticketResponse);
        }

        /// <summary>
        /// Changes the priority of the specified ticket to the provided value.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket whose priority will be changed.
        /// </param>
        /// <param name="priority">
        /// Priority level to set for the ticket (Low, Medium, High, Critical).
        /// </param>
        [Authorize(Roles = "Admin,Analyst")]
        [HttpPut("priority/{ticketId}")]
        public async Task<IActionResult> ChangeTicketPriority([FromRoute] Guid ticketId, [FromBody] TicketPriority priority)
        {
            var ticketResponse = await _ticketService.ChangeTicketPriority(ticketId, priority);
            return Ok(ticketResponse);
        }
    }
}
