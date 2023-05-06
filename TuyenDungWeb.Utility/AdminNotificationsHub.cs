using Microsoft.AspNetCore.SignalR;

namespace TuyenDungWeb.Utility
{
    public class AdminNotificationsHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
