using Chamados.Enums;
using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Tickets
{
    /// <summary>
    /// Represents the data returned when listing tickets in the system.
    /// </summary>
    public class TicketListDto
    {
        public Guid TicketId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public DateTime Created { get; set; }
        public TicketStatus Status { get; set; }
        public string? AssignedToUserName { get; set; }
    }
}
