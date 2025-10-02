using Microsoft.AspNetCore.SignalR;
using signalrTask.Context;
using signalrTask.Models;

namespace signalrTask.Hubs
{
    public class displayEmpHub : Hub
    {
        public static List<string> Ids { get; set; } = new List<string>();
        public override Task OnConnectedAsync()
        {
            Ids.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

    }
}
