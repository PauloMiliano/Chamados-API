using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Tickets
{
    public class AssignTicketDto
    {
        [Required(ErrorMessage = "O Id do Ticket é obrigátorio.")]
        public Guid TicketId { get; set; }
    }
}
