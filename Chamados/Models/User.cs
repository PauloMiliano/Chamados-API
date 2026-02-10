using Chamados.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Chamados.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }

    }
}
