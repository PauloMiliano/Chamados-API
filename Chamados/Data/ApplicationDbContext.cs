using Chamados.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chamados.Data
{
    public class ApplicationDbContext
    : IdentityDbContext<User> 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }

        }
}
