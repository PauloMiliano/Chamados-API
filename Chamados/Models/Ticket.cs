using Chamados.Enums;
using System.ComponentModel.DataAnnotations;

namespace Chamados.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O campo título é obrigátorio.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "A descrição do Ticket é obrigatória.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "O Id do usuário é obrigatório.")]
        public string AuthorId { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "O campo de prioridade do Ticket é obrigatório.")]
        public TicketPriority Priority { get; set; }
        [Required(ErrorMessage = "O campo Status do Ticket é obrigatório.")]
        public TicketStatus Status { get; set; }
        public string? AssignedToUserId { get; set; }

    }
}
