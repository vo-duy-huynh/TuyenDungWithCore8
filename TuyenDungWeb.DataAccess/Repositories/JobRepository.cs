using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        private ApplicationDbContext _db;
        public JobRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Tag obj)
        {
            _db.Tags.Update(obj);
        }
    }
}
