

using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Tickets
{
    public class TicketResponse
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        public string AuthorName { get; set; }
        [Required(ErrorMessage = "O título do Ticket é obrigatório.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "A descrição do Ticket é obrigatória.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "A data de criação do Ticket é obrigatória.")]
        public DateTime Date { get; set; }
    }
}
