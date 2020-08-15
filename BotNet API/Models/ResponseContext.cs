using Microsoft.EntityFrameworkCore;

namespace BotNet_API.Models
{
    public class ResponseContext : DbContext
    {
        public ResponseContext(DbContextOptions<ResponseContext> options) : base(options) { }

        public DbSet<Response> Responses { get; set; }
    }
}
