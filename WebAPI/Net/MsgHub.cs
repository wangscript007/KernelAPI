using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    [Authorize]
    public class MsgHub : Hub
    {
        public async Task SendMessage(object msg)
        {
            await Clients.All.SendAsync("ReceiveMessage", msg);
        }
    }
}
