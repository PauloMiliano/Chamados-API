using Chamados.Enums;
using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Tickets
{
    public class TicketListDto
    {
        public Guid TicketId { get; set; }
        [Required(ErrorMessage = "O campo de título é obrigatório.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "O Id do usuário é obrigatório.")]
        public string AuthorId { get; set; }
        [Required(ErrorMessage = "O nome do usuário é obrigatório")]
        public string AuthorName { get; set; }
        [Required(ErrorMessage = "A data de criação é obrigatória.")]
        public DateTime Created { get; set; }
        [Required(ErrorMessage = "O Status do Ticket é obrigatório")]
        public TicketStatus Status { get; set; }
        public string? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
    }
}
