namespace Chamados.DTOs.Users
{
    public class GetUserResponseDto
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
