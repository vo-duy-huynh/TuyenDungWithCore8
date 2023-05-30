using Microsoft.AspNetCore.SignalR;
using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Notification;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public interface INotificationService
    {
        void CreateAdminNotification(string message, int notificationCount, string userIdReceive);
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

        public void CreateAdminNotification(string message, int notificationCount, string userIdReceice)
        {
            List<JobPostTemp> adminNotifications = _dbContext.JobPostTemps.OrderByDescending(n => n.CreatedDate).Where(n => n.IsApprove == false).ToList();
            //take message and create date from adminNotifications
            List<string> messageList = new List<string>();
            foreach (var item in adminNotifications)
            {
                messageList.Add(item.Message);
            }
            string loaiThongBao = "thông báo đăng tuyển";

            _hubContext.Clients.User(userIdReceice).SendAsync("ReceiveNotification", message, notificationCount, messageList, loaiThongBao);
        }
        public void CreateNotificationForCompanyAfterAdmin(string userIdReceive)
        {
            List<JobPostTemp> adminNotifications = _dbContext.JobPostTemps.OrderByDescending(n => n.CreatedDate).Where(n => n.IsApprove == true).ToList();
            //take message and create date from adminNotifications
            List<string> messageList = new List<string>();
            foreach (var item in adminNotifications)
            {
                messageList.Add(item.Message);
            }
            int notificationCount = adminNotifications.Count();
            string message = "Bạn có " + notificationCount + " thông báo mới";
            string loaiThongBao = "thông báo duyệt đăng tuyển";
            _hubContext.Clients.User(userIdReceive).SendAsync("ReceiveNotification", message, notificationCount, messageList, loaiThongBao);
        }
        public void CreateNotificationForCompany(string userIdReceive)
        {
            List<ProfileHeader> companyNotifications = _dbContext.ProfileHeaders.OrderBy(n => n.ApplyDate).Where(n => n.UserReceiveId == userIdReceive && n.Status == "Pending").ToList();
            List<string> messageList = new List<string>();
            foreach (var item in companyNotifications)
            {
                messageList.Add(item.Name + " đã ứng tuyển vào " + item.JobTitle);
            }
            int notificationCount = companyNotifications.Count();
            string message = "Bạn có " + notificationCount + " ứng viên mới";
            string loaiThongBao = "thông báo ứng tuyển";
            _hubContext.Clients.User(userIdReceive).SendAsync("ReceiveNotification", message, notificationCount, messageList, loaiThongBao);
        }
        public void CreateNotificationForUser(string userIdReceive)
        {
            List<ProfileHeader> userNotifications = _dbContext.ProfileHeaders.OrderBy(n => n.ApplyDate).Where(n => n.UserReceiveId == userIdReceive && n.Status != "Pending").ToList();
            List<string> messageList = new List<string>();
            foreach (var item in userNotifications)
            {
                //get jobPost
                var jobPost = _dbContext.JobPosts.FirstOrDefault(n => n.Id == item.JobPostId);
                //get company
                var company = _dbContext.Companies.FirstOrDefault(n => n.Id == jobPost.CompanyId);
                if (item.Status == "Approved")
                {
                    messageList.Add("Bạn đã trúng tuyển vào " + company.Name + " vị trí " + item.JobTitle);
                }
                else if (item.Status == "Rejected")
                {
                    messageList.Add("Bạn có thể ứng tuyển vào vị trí khác của " + company.Name);
                }
                else if (item.Status == "Interview")
                {
                    messageList.Add("Hãy chuẩn bị để phỏng vấn " + item.JobTitle + " tại " + company.Name);
                }
            }
            int notificationCount = userNotifications.Count();
            string message = "Bạn có " + notificationCount + " ứng tuyển mới";
            string loaiThongBao = "thông báo duyệt ứng tuyển";
            _hubContext.Clients.User(userIdReceive).SendAsync("ReceiveNotification", message, notificationCount, messageList, loaiThongBao);
        }
        //create chat all user
        public void CreateChatAllUser(string userId, string message)
        {
            //get fullname
            var user = _dbContext.ApplicationUsers.FirstOrDefault(n => n.Id == userId);
            List<string> messageList = new List<string>();
            messageList.Add(message);
            message = user.FullName;
            int notificationCount = 1;
            string loaiThongBao = "tin nhắn";
            _hubContext.Clients.All.SendAsync("ReceiveNotification", message, notificationCount, messageList, loaiThongBao);
        }

        public int GetUnreadNotificationCountForUser(string? userId)
        {
            return _dbContext.JobPostTemps.Where(n => n.UserIdReceive == userId).Count();
        }
    }


}
