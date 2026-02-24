using Chamados.Enums;

namespace Chamados.Models
{
    public class TicketHistory
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        public TicketActions Action { get; set; }
        public string PerformedByUserId { get; set; }
        public DateTime PerformedAt { get; set; }
    }
}
