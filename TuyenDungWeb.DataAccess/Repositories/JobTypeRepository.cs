using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class JobTypeRepository : Repository<JobType>, IJobTypeRepository
    {
        private ApplicationDbContext _db;
        public JobTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
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
