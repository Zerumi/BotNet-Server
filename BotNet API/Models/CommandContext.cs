using Microsoft.EntityFrameworkCore;

namespace BotNet_API.Models
{
    public class CommandContext : DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> options) : base(options) { }

        public DbSet<Command> Commands { get; set; }
    }
}