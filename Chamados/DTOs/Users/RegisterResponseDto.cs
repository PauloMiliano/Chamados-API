namespace Chamados.DTOs.Users
{
    public class RegisterResponseDto
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Message { get; set; }
    }
}
