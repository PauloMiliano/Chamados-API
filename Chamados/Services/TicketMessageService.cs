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
