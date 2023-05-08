using Microsoft.AspNetCore.SignalR;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.DataAccess.Notification
{
    public class AdminNotificationsHub : Hub
    {
        private readonly NotificationService _notificationService;

        public AdminNotificationsHub(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task SendNotificationToAdmin(string message)
        {
            await Clients.User(SD.Role_Admin).SendAsync("ReceiveNotification", message);
        }

        public override async Task OnConnectedAsync()
        {
            var adminUser = Context.User;
            if (adminUser != null && adminUser.Identity != null && adminUser.Identity.IsAuthenticated && adminUser.IsInRole(SD.Role_Admin))
            {
                var adminUserName = adminUser.Identity.Name;
                int notificationCount = _notificationService.GetUnreadNotificationCountForUser();
                await Clients.User(adminUserName).SendAsync("ReceiveNotification", "", notificationCount);
            }
            await base.OnConnectedAsync();
        }
    }
}
