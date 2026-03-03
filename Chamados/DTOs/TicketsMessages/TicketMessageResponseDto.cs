namespace Chamados.DTOs.TicketsMessages
{
    /// <summary>
    /// Represents the data returned when retrieving messages for a ticket in the system.
    /// </summary>
    public class TicketMessageResponseDto
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
