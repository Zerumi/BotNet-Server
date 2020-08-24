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

        public delegate void ListenResponseHandler(Response response);

        public static event ListenResponseHandler OnResponseAdded;

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<Response>> PostResponse([FromBody] Response item)
        {
            _context.Responses.Add(item);
            await _context.SaveChangesAsync();

            OnResponseAdded?.Invoke(item);

            return Created("api/response", item);
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