using System.ComponentModel.DataAnnotations;

namespace Chamados.DTOs.TicketsMessages
{
    /// <summary>
    /// Represents the data required to create a new message for a ticket in the system.
    /// </summary>
    public class CreateTicketMessageDto
    {
        [Required(ErrorMessage = "O campo mensagem e obrigatório.")]
        public string Message { get; set; }
    }
}
