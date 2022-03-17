using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.SignalRHubs
{
    public class UsersHub : Hub
    {
        private IUsersService _usersService;

        public UsersHub(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
            await Clients.All.SendAsync("New Client Arrived!", Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task WelcomeMessage(string message)
        {
            await Clients.AllExcept(new[] { Context.ConnectionId }).SendAsync(nameof(WelcomeMessage), message);
        }
    }
}
