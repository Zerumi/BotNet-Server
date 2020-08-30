// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.IO;
using BotNet_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BotNet_API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost]
        public ActionResult<bool> PostAdmin([FromBody] BotNet_API.Models.Auth pass)
        {
            return Encryption.Decrypt(pass.password) == new StreamReader(System.IO.File.OpenRead(@".\password.txt")).ReadToEnd();
        }
    }
}