namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface INotificationRepository : IRepository<TuyenDungWeb.Models.Notification>
    {
        void Update(TuyenDungWeb.Models.Notification obj);
    }
}
