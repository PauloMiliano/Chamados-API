

using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.Tickets
{
    public class TicketResponse
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
