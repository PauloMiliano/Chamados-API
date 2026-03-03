using Chamados.Data;
using Chamados.DTOs.TicketsMessages;
using Chamados.Enums;
using Chamados.Exceptions;
using Chamados.Interfaces;
using Chamados.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace Chamados.Services
{
    public class TicketMessageService : ITicketMessageService
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TicketMessageService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Creates a new message for a given ticket. Validates that the ticket exists and is not closed before allowing the message to be created.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket to which the message will be added.
        /// </param>
        /// <param name="senderId">
        /// Identifier of the user sending the message.
        /// </param>
        /// <param name="messageRequest">
        /// Contains the content of the message to be created.
        /// </param>
        /// <returns>
        /// The details of the created message, including the sender's name, message content, and timestamp.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown when the specified ticket does not exist, indicating that the message cannot be sent to a non-existent ticket.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified ticket is closed, indicating that messages cannot be sent to a closed ticket.
        /// </exception>
        public async Task<TicketMessageResponseDto> CreateMessageAsync(Guid ticketId, string senderId, CreateTicketMessageDto messageRequest)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado, não foi possível enviar sua mensagem.");
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                throw new InvalidOperationException("Não é possível enviar mensagens para um ticket fechado.");
            }

            var message = new TicketMessage
            {
                SenderId = senderId,
                TicketId = ticketId,
                Message = messageRequest.Message,
                SentAt = DateTime.UtcNow
            };

            await _context.TicketMessages.AddAsync(message);

            var ticketHistory = new TicketHistory
            {
                TicketId = ticketId,
                Action = TicketActions.Answered,
                PerformedByUserId = senderId,
                PerformedAt = DateTime.UtcNow
            };
            await _context.TicketHistories.AddAsync(ticketHistory);
            await _context.SaveChangesAsync();
            var sender = await _userManager.FindByIdAsync(senderId);

            return new TicketMessageResponseDto
            {
                Id = message.Id,
                SenderName = sender.Name,
                Message = message.Message,
                SentAt = message.SentAt
            };
        }

        /// <summary>
        /// Gets all messages for a given ticket, ordered by the time they were sent.
        /// </summary>
        /// <param name="ticketId">
        /// Identifier of the ticket for which to retrieve messages.
        /// </param>
        /// <returns>
        /// The list of messages associated with the specified ticket.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown when the specified ticket does not exist.
        /// </exception>
        public async Task<List<TicketMessageResponseDto>> GetMessagesAsync(Guid ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
            {
                throw new NotFoundException("Ticket não encontrado, não foi possível recuperar as mensagens.");
            }

            var messages = await _context.TicketMessages
                .Where(m => m.TicketId == ticketId)
                .OrderBy(m => m.SentAt)
                .Select(m => new TicketMessageResponseDto
                {
                    Id = m.Id,
                    SenderName = m.Sender.Name,
                    Message = m.Message,
                    SentAt = m.SentAt
                }).ToListAsync();

            return messages;
        }
    }
}
