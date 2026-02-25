namespace Chamados.DTOs.TicketsMessages
{
    public class TicketMessageResponseDto
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
