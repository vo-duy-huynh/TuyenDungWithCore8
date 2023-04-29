using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface INotificationRepository : IRepository<Notification>
    {
        void Update(Notification obj);
    }
}
