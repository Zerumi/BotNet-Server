using BotNet_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BotNet_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Info> GetInfo()
        {
            var inf = new Info()
            {
                clients = 0,
                messages = 0,
                environment = "development",
                port = 44316,
                uri = "http://localhost:44316/",
                version = 2
            };
            return inf;
        }
    }
}