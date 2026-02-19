using Chamados.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Chamados.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        public string Name { get; set; }

    }
}
