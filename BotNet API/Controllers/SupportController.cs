// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using BotNet_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BotNet_API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        public VerifyVersion[] versions = new VerifyVersion[]
            {
                new VerifyVersion()
                {
                    version = "1.5.0.0",
                    isDeprecated = true,
                    isNotLatest = true,
                    isUpdateNeeded = true,
                    cmdlib = new string[] {"2.1.0.0"},
                    m3md2 = new string[] {"1.3.0.0"},
                    m3md2_startup = new string[] {"1.1.0.0"}
                },
                new VerifyVersion()
                {
                    version = "1.6.0.0",
                    isDeprecated = false,
                    isNotLatest = false, // after release true
                    isUpdateNeeded = false, // after release true
                    cmdlib = new string[] {"2.1.0.0"},
                    m3md2 = new string[] {"1.3.0.0"},
                    m3md2_startup = new string[] {"1.2.0.0"}
                },
                new VerifyVersion()
                {
                    version = "2.0.0.0",
                    isDeprecated = false,
                    isNotLatest = false,
                    isUpdateNeeded = false,
                    cmdlib = new string[] {"2.1.0.0", "2.2.0.0"},
                    m3md2 = new string[] {"1.4.0.0"},
                    m3md2_startup = new string[] {"1.3.0.0"},
                    custommessage = "Доступ к авторизации и скриншотам восстановлен, однако прослушка и диагностика API недоступны. Мы работаем над исправлением..."
                }
            };
        [HttpGet("versions/{version}")]
        public ActionResult<VerifyVersion> GetVersions(string version)
        {
            return Array.Find(versions, x => x.version == version);
        }
        [HttpGet("version_note/{version}")]
        public ActionResult<string> GetVersionNote(string version)
        {
            return Array.Find(versions, x => x.version == version).custommessage;
        }
    }
}