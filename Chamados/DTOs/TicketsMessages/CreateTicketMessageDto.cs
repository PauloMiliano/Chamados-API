using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.TicketsMessages
{
    public class CreateTicketMessageDto
    {
        [Required(ErrorMessage = "O campo mensagem e obrigatório.")]
        public string Message { get; set; }
    }
}
