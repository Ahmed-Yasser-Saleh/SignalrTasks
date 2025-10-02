using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using signalrTask.Context;
using signalrTask.Models;

namespace signalrTask.Hubs
{
    public class ChatHub : Hub
    {
        MyContext db;
        public ChatHub(MyContext db)
        {
            this.db = db;
        }
        public override async Task OnConnectedAsync()
        {
            var groupnames = db.UserGroups.Where(e => e.Username == Context.User.Identity.Name).Select(e => e.Groupname).ToList();
            if (groupnames != null)
            {
                foreach (var group in groupnames)
                {
                   await Groups.AddToGroupAsync(Context.ConnectionId, group);
                }
            }
            
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? ex)
        {
           await base.OnDisconnectedAsync(ex);
        } 
        public async Task joingroup(string groupName, string username) {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            db.UserGroups.Add(new UserGroup { Groupname = groupName, Username = username });
            db.SaveChanges();
            await Clients.GroupExcept(groupName, Context.ConnectionId).SendAsync("JoinGroup", groupName, username);
        }

        public async Task sendmessage(string message, string username) {
                var groupnames = db.UserGroups.Where(e => e.Username == username).Select(e => e.Groupname).ToList();  
                await  Clients.Groups(groupnames).SendAsync("recievemessage", message, username);
        }
    }
}
