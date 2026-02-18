using Chamados.Enums;
using System.ComponentModel.DataAnnotations;

namespace Chamados.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [Required]
        public TicketPriority Priority { get; set; }
        [Required]
        public TicketStatus Status { get; set; }
        public Guid? AssignedToUserId { get; set; }

    }
}
