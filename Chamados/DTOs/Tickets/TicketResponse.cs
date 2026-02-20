

using Chamados.Enums;
using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Tickets
{
    public class TicketResponse
    {
        public Guid Id { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string? AssignedToUserName { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
