using Microsoft.AspNetCore.SignalR;
using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.Models;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public interface INotificationService
    {
        void CreateAdminNotification(string message);
    }

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHubContext<AdminNotificationsHub> _hubContext;

        public NotificationService(ApplicationDbContext dbContext, IHubContext<AdminNotificationsHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        public void CreateAdminNotification(string message)
        {
            var notification = new AdminNotification
            {
                Message = message,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.AdminNotifications.Add(notification);
            _dbContext.SaveChanges();

            var notificationCount = _dbContext.AdminNotifications.Count(n => !n.IsRead);

            _hubContext.Clients.All.SendAsync("ReceiveNotification", message, notificationCount);
        }
    }


}
