using Microsoft.AspNetCore.SignalR;
using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.DataAccess.Notification
{
    public class AdminNotificationsHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly NotificationService _notificationService;

        public AdminNotificationsHub(NotificationService notificationService, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }
        public async Task SendMessage(string user, string message)
        {
            //get fullname
            string? fullName = _dbContext.ApplicationUsers.FirstOrDefault(n => n.Id == user)?.FullName;
            await Clients.All.SendAsync("ReceiveMessage", fullName, message);
        }
        public override async Task OnConnectedAsync()
        {
            var adminUser = Context.User;
            if (adminUser != null && adminUser.Identity != null && adminUser.Identity.IsAuthenticated && adminUser.IsInRole(SD.Role_Admin))
            {
                var adminUserName = adminUser.Identity.Name;
                //get userId
                string? userId = adminUser.Claims.FirstOrDefault()?.Value;
                int notificationCount = _notificationService.GetUnreadNotificationCountForUser(userId);
                await Clients.User(adminUserName).SendAsync("ReceiveNotificationCount", notificationCount);
            }
            else if (adminUser != null && adminUser.Identity != null && adminUser.Identity.IsAuthenticated && adminUser.IsInRole(SD.Role_Company))
            {
                var adminUserName = adminUser.Identity.Name;
                //get userId
                string? userId = _dbContext.ApplicationUsers.FirstOrDefault(n => n.Email == adminUserName)?.Id;
                _notificationService.CreateNotificationForCompany(userId);
                _notificationService.CreateNotificationForCompanyAfterAdmin(userId);
            }
            else if (adminUser != null && adminUser.Identity != null && adminUser.Identity.IsAuthenticated && adminUser.IsInRole(SD.Role_Customer))
            {
                var adminUserName = adminUser.Identity.Name;
                //get userId
                string? userId = _dbContext.ApplicationUsers.FirstOrDefault(n => n.Email == adminUserName)?.Id;
                _notificationService.CreateNotificationForUser(userId);
            }
            await base.OnConnectedAsync();
        }
    }
}
