using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotNet_API.Models;
using Microsoft.Extensions.Primitives;

namespace BotNet_API.Hubs
{
    public class ResponseHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {

        }

        public async Task SendResponse(Response response)
        {
            await Clients.All.SendAsync("ReceiveResponse", response);
        }
    }
}