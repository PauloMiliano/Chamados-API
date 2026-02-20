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
        public async Task<IActionResult> OpenTicket([FromBody] TicketRequestDto ticket)
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

    }
}
