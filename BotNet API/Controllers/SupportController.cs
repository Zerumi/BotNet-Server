﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotNet_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                    custommessage = "Доступ к авторизации восстановлен, однако скриншоты и диагностика API недоступны. Мы работаем над исправлением..."
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
            return JsonConvert.SerializeObject(Array.Find(versions, x => x.version == version).custommessage);
        }
    }
}