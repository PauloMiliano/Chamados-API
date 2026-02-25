using Chamados.DTOs.Tickets;
using Chamados.DTOs.TicketsMessages;
using Chamados.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chamados.Controllers
{
    [ApiController]
    [Route("api/tickets/{ticketId}/messages")]
    [Authorize]
    public class TicketMessageController : Controller
    {

        private readonly ITicketMessageService _messageService;

        public TicketMessageController(ITicketMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public IActionResult GetMessagesByTicketId([FromRoute] Guid ticketId)
        {
            var messageRequest = _messageService.GetMessagesAsync(ticketId);
            return Ok(messageRequest);
        }

        [HttpPost]
        public IActionResult AddMessageToTicket([FromRoute] Guid ticketId, [FromBody] CreateTicketMessageDto message)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messageRequest = _messageService.CreateMessageAsync(ticketId, senderId, message);
            return Ok(messageRequest);
        }
    }
}
