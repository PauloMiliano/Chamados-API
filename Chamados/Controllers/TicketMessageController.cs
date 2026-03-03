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

        /// <summary>
        /// Gets all messages associated with a specific ticket, identified by its ID.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket for which to retrieve messages. This ID is passed as a route parameter in the URL.
        /// </param>
        [HttpGet]
        [Authorize(Roles = "User,Admin,Analyst")]
        public async Task<IActionResult> GetMessagesByTicketId([FromRoute] Guid ticketId)
        {
            var messageRequest = await _messageService.GetMessagesAsync(ticketId);
            return Ok(messageRequest);
        }

        /// <summary>
        /// Ads a new message to a specific ticket, identified by its ID.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket to which the message will be added. This ID is passed as a route parameter in the URL.
        /// </param>
        /// <param name="message">
        /// Message content to be added to the ticket.
        /// </param>
        [HttpPost]
        [Authorize(Roles = "User,Admin,Analyst")]
        public async Task<IActionResult> AddMessageToTicket([FromRoute] Guid ticketId, [FromBody] CreateTicketMessageDto message)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messageRequest = await _messageService.CreateMessageAsync(ticketId, senderId, message);
            return Ok(messageRequest);
        }
    }
}
