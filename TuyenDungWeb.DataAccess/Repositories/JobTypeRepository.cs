using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class JobTypeRepository : Repository<JobType>, IJobTypeRepository
    {
        private ApplicationDbContext _db;
        public JobTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public JobType GetById(int? id)
        {
            var job = _db.JobTypes.Find(id);
            return job;
        }

        public void Update(JobType obj)
        {
            _db.JobTypes.Update(obj);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
