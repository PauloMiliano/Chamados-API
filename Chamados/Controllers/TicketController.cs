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

        [Authorize(Roles = "User, Analyst, Admin")]
        [HttpPost("open")]
        public async Task<IActionResult> OpenTicket([FromBody] CreateTicketDto ticket)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticketResponse = await _ticketService.OpenTicket(ticket, userId);
            return Ok(ticketResponse);
        }

        [Authorize(Roles = "Admin,Analyst,User")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTickets([FromQuery] int pageNumber, int pageSize, TicketStatus? status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var tickets = await _ticketService.GetAllTickets(pageNumber, pageSize, userId, userRole, status);
            return Ok(tickets);
        }

        [Authorize(Roles = "Admin, Analyst")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById([FromRoute] Guid ticketId)
        {
            var ticket = await _ticketService.GetTicketById(ticketId);
            return Ok(ticket);
        }

        [Authorize(Roles = "Admin, Analyst")]
        [HttpPut("assign/{ticketId}")]
        public async Task<IActionResult> AssignUserTicket([FromRoute] Guid ticketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticketResponse = await _ticketService.AssignUserTicket(ticketId, userId);
            return Ok(ticketResponse);
        }

        [Authorize(Roles = "Admin, Analyst")]
        [HttpPut("close/{ticketId}")]
        public async Task<IActionResult> CloseTicket([FromRoute] Guid ticketId)
        {
            var ticketResponse = await _ticketService.CloseTicket(ticketId);
            return Ok(ticketResponse);
        }

    }
}
