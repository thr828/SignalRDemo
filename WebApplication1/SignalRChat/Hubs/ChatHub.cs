using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            //触发所有客户端定义的"ReceiveMessage"方法
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
