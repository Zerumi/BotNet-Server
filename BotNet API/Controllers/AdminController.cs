using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BotNet_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BotNet_API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost]
        public ActionResult<bool> PostAdmin([FromBody] Auth pass)
        {
            return Encryption.Decrypt(pass.password) == new StreamReader(System.IO.File.OpenRead(@".\password.txt")).ReadToEnd();
        }
    }
}
