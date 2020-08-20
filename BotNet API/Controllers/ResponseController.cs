// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotNet_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BotNet_API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly ResponseContext _context;

        public ResponseController(ResponseContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Response>>> GetResponses()
        {
            return await _context.Responses.ToListAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetResponse(uint id)
        {
            var response = await _context.Responses.FindAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return response;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<Response>> PostResponse([FromBody] Response item)
        {
            _context.Responses.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResponse), new { item.id }, item);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteResponses()
        {
            _context.Responses.RemoveRange(_context.Responses.ToArray());
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}