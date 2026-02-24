using Chamados.Enums;

namespace Chamados.DTOs.Tickets
{
    public class AssignTicketDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AssignedToUserName { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
