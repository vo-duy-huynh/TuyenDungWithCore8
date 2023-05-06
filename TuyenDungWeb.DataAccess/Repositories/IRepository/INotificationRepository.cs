using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface INotificationRepository : IRepository<AdminNotification>
    {
        void Update(AdminNotification obj);
    }
}
