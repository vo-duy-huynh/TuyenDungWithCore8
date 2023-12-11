using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class NotificationRepository : Repository<TuyenDungWeb.Models.Notification>, INotificationRepository
    {
        private ApplicationDbContext _db;
        public NotificationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(TuyenDungWeb.Models.Notification obj)
        {
            _db.Notifications.Update(obj);
        }
    }
}
