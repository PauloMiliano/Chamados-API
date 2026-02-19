using Chamados.DTOs.Tickets;
using Chamados.Interfaces;
using Chamados.Models;
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
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var ticketResponse = await _ticketService.OpenTicket(ticket, userId);
                return Ok(ticketResponse);

            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao realizar login. Exceção: " + ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Analyst")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTickets([FromQuery] int pageNumber, int pageSize)
        {
            try
            {
                var tickets = await _ticketService.GetAllTickets(pageNumber, pageSize);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao obter tickets. Exceção: " + ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Analyst")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById([FromRoute] Guid id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketById(id);
                if (ticket == null)
                {
                    return NotFound("Ticket não encontrado.");
                }
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao obter ticket. Exceção: " + ex.Message);
            }
        }

    }
}
