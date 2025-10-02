using Microsoft.AspNetCore.SignalR;

namespace signalrTask.Hubs
{
    public class MoveDivHub : Hub
    {
        public async Task UpdatePosition(int x, int y)
        {
            await Clients.All.SendAsync("MoveDiv", x, y);
        }
    }
}
