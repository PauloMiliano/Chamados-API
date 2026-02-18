using Chamados.DTOs.Tickets;
using Chamados.Interfaces;
using Chamados.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> OpenTicket(TicketRequestDto ticket)
        {
            try
            {
                var ticketResponse = await _ticketService.OpenTicket(ticket);
                return Ok(ticketResponse);

            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao realizar login. Exceção: " + ex.Message);
            }
        }

    }
}
