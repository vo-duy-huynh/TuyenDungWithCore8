using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        private ApplicationDbContext _db;
        public JobRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public Job GetById(int id)
        {
            var job = _db.Jobs.Find(id);
            return job;
        }

        public void Update(Job obj)
        {
            _db.Jobs.Update(obj);
        }
    }
}
