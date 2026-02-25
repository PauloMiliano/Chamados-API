using Chamados.Enums;
using System.ComponentModel.DataAnnotations;

namespace Chamados.Models
{
    public class TicketHistory
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O Id do Ticket é obrigatório.")]
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        [Required(ErrorMessage = "A ação realizada é obrigatória.")]
        public TicketActions Action { get; set; }
        [Required(ErrorMessage = "Id do usuário é obrigatório.")]
        public string PerformedByUserId { get; set; }
        [Required(ErrorMessage = "Data da ação é obrigatória.")]
        public DateTime PerformedAt { get; set; }
    }
}
