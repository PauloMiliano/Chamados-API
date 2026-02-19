using Chamados.Enums;
using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Tickets
{
    public class TicketRequestDto
    {
        [Required(ErrorMessage = "O título do Ticket é obrigatório.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "A descrição do Ticket é obrigatório.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "A prioridade do Ticket é obrigatória.")]
        public TicketPriority Priority { get; set; }
        [Required(ErrorMessage = "O Status do Ticket é obrigatório.")]
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
