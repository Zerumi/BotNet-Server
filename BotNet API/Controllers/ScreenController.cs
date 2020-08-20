// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.Collections.Generic;
using System.Threading.Tasks;
using BotNet_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BotNet_API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ScreenController : ControllerBase
    {
        private readonly ScreenContext _context;

        public ScreenController(ScreenContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Screen>>> GetScreens()
        {
            return await _context.Screens.Include(x => x.screens).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Screen>> GetScreen(uint id)
        {
            return (await _context.Screens.Include(x => x.screens).ToListAsync()).Find(x => x.id == id);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> PostScreen([FromBody] ScreenByte screen, uint id)
        {
            Screen userscreen = await _context.Screens.FindAsync(id) ?? _context.Screens.Add(new Screen() {id = id}).Entity;
            if (userscreen.screens == null)
            {
                userscreen.screens = new List<ScreenByte>();
            }
            userscreen.screens.Add(screen);
            _ = await _context.SaveChangesAsync();
            return Created($"/api/screen/{id}", screen);
        }
    }
}