

namespace Chamados.DTOs.Tickets
{
    public class TicketResponse
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool Success { get; set; } 
    }
}
