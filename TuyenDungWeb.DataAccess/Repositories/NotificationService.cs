using Microsoft.AspNetCore.SignalR;
using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Notification;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public interface INotificationService
    {
        void CreateAdminNotification(string message, int notificationCount);
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

        public void CreateAdminNotification(string message, int notificationCount)
        {
            List<JobPostTemp> adminNotifications = _dbContext.JobPostTemps.OrderBy(n => n.CreatedDate).Where(n => n.IsApprove == false).ToList();
            //take message and create date from adminNotifications
            List<string> messageList = new List<string>();
            foreach (var item in adminNotifications)
            {
                messageList.Add(item.Message);
            }


            _hubContext.Clients.All.SendAsync("ReceiveNotification", message, notificationCount, messageList);
        }

        public int GetUnreadNotificationCountForUser()
        {
            return _dbContext.JobPostTemps.Where(n => n.IsApprove == false).Count();
        }
    }


}
