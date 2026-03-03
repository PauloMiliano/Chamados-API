using Chamados.Enums;

namespace Chamados.DTOs.Tickets
{
    /// <summary>
    /// Represents the data returned when performing actions on a ticket, such as assigning a user or reopening a ticket.
    /// </summary>
    public class TicketActionsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AssignedToUserName { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
