using Chamados.Enums;

namespace Chamados.DTOs.Tickets
{
    public class TicketRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
