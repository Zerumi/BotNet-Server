using Microsoft.EntityFrameworkCore;

namespace BotNet_API.Models
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options) : base(options)        {        }

        public DbSet<Client> Clients { get; set; }
    }
}