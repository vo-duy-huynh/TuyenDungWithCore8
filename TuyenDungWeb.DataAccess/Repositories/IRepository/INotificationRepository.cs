using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface INotificationRepository : IRepository<Notification>
    {
        void Update(Notification obj);
    }
}
