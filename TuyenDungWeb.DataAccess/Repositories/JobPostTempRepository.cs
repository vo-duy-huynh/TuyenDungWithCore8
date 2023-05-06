using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class JobPostTempRepository : Repository<JobPostTemp>, IJobPostTempRepository
    {
        private ApplicationDbContext _db;
        public JobPostTempRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(JobPostTemp obj)
        {
            var objFromDb = _db.JobPostTemps.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Content = obj.Content;
                objFromDb.Heading = obj.Heading;
                objFromDb.ShortDescription = obj.ShortDescription;
                objFromDb.CreatedDate = DateTime.Now;
                objFromDb.EndDate = obj.EndDate;
                objFromDb.NumberOfRecruiting = obj.NumberOfRecruiting;
                objFromDb.Experience = obj.Experience;
                objFromDb.Gender = obj.Gender;
                objFromDb.Salary = obj.Salary;
                objFromDb.Location = obj.Location;
            }
        }
    }
}
