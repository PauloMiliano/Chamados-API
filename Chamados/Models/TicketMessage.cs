using System.ComponentModel.DataAnnotations;

namespace Chamados.Models
{
    public class TicketMessage
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Id do remetente é obrigatório.")]
        public string SenderId { get; set; }
        public User? Sender { get; set; }

        [Required(ErrorMessage = "O Id do ticket é obrigatório.")]
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        [Required(ErrorMessage = "O campo de mensagem é obrigatório.")]
        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
