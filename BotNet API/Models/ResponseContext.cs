// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using Microsoft.EntityFrameworkCore;

namespace BotNet_API.Models
{
    public class ResponseContext : DbContext
    {
        public ResponseContext(DbContextOptions<ResponseContext> options) : base(options) { }

        public DbSet<Response> Responses { get; set; }
    }
}