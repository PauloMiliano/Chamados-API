using Chamados.DTOs.Tickets;
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

        [HttpPost("open")]
        public async Task<IActionResult> OpenTicket([FromBody] CreateTicketDto ticket)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticketResponse = await _ticketService.OpenTicket(ticket, userId);
            return Ok(ticketResponse);
        }

        [Authorize(Roles = "Admin,Analyst")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTickets([FromQuery] int pageNumber, int pageSize)
        {
            var tickets = await _ticketService.GetAllTickets(pageNumber, pageSize);
            return Ok(tickets);
        }

        [Authorize(Roles = "Admin, Analyst")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById([FromRoute] Guid id)
        {
            var ticket = await _ticketService.GetTicketById(id);
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
